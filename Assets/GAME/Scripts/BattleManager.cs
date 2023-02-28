using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class BattleManager : MonoBehaviour
{
  public static Action<string> OnBattleEnd;

  [SerializeField] private GameObject _charactercardPrefab;
  [SerializeField] private Transform _playerCardParent;
  [SerializeField] private Transform _enemyCardParent;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;
  [SerializeField] private CameraShake _cameraShake;

  private List<CharacterCard> _playerCards = new List<CharacterCard>();
  private List<CharacterCard> _enemyCards = new List<CharacterCard>();

  private bool battleEnded = false;

  // void OnEnable()
  // {
  //   CharacterCard.SummonMinion += HandleSummon;
  //   CharacterCard.DamageRandomEnemy += DamageRandomEnemy;
  // }

  // void OnDisable()
  // {
  //   CharacterCard.SummonMinion -= HandleSummon;
  //   CharacterCard.DamageRandomEnemy -= DamageRandomEnemy;
  // }

  private void HandleSummon(Character character, bool enemy)
  {
    Summon(character, true, enemy);
  }

  private void DamageRandomEnemy(int damage, bool enemy)
  {
    StartCoroutine(DamageRandomEnemy(damage, 1.5f, enemy));
  }

  IEnumerator DamageRandomEnemy(int damage, float delay, bool enemy)
  {
    var cardsToDamage = enemy ? _playerCards : _enemyCards;//player damages enemy cards
    if (cardsToDamage.Count == 0)
    {
      Debug.Log("DamageRandomEnemy: No cards to damage");
      yield break;
    }
    int random = UnityEngine.Random.Range(0, cardsToDamage.Count);
    Debug.Log("DamageRandomEnemy: " + damage + " random: " + random);
    yield return new WaitForSeconds(delay);
    CharacterCard cardToDamage = cardsToDamage[random];
    // enemyCharacter.Health -= damage;
    cardToDamage.TakeDamage(damage);

    if (cardToDamage.Health <= 0)
    {
      cardToDamage.SetAnimation("die");
      yield return new WaitForSeconds(1.0f);
      // if (enemy)
      //   _playerCards.Remove(cardToDamage);
      // else
      //   _enemyCards.Remove(enemyCharacter);
      // // _enemyCards.Remove(enemyCharacter);
      cardsToDamage.Remove(cardToDamage);
      cardToDamage.MinionDefeated();
      Ready();
    }
    yield return null;
  }

  void Start()
  {
    CharacterCard.SummonMinion = HandleSummon;
    CharacterCard.DamageRandomEnemy = DamageRandomEnemy;

    if (BattleData._playerCharacters.Count == 0) GetComponent<BattleData>().Test();
    GetComponent<BattleData>().PopulateEnemies();
    PopulateBattle();
    //Do any start of battle abilities here
    foreach (CharacterCard card in _playerCards)
    {
      card.StartOfBattle();
    }
    foreach (CharacterCard card in _enemyCards)
    {
      card.StartOfBattle();
    }
    Ready();
  }

  void PopulateBattle()
  {
    foreach (Transform child in _playerCardParent)
    {
      Destroy(child.gameObject);
    }
    foreach (Transform child in _enemyCardParent)
    {
      Destroy(child.gameObject);
    }
    // Get the player characters
    foreach (Character character in BattleData._playerCharacters)
    {
      Summon(character);
    }
    // Get the enemy characters
    foreach (Character character in BattleData._enemyCharacters)
    {
      Summon(character, false, true);
    }
  }

  public void Summon(Character character, bool front = false, bool enemy = false)
  {
    Debug.Log("Summon: " + character.name + " " + front + " " + enemy);
    GameObject card = Instantiate(_charactercardPrefab, enemy ? _enemyCardParent : _playerCardParent);
    card.GetComponent<CharacterCardDrag>().enabled = false;//Turn off dragging
    card.GetComponent<CharacterCard>().SetCharacter(character, enemy);
    if (front)
    {
      (enemy ? _enemyCards : _playerCards).Insert(0, card.GetComponent<CharacterCard>());
      card.transform.SetAsFirstSibling();
    }
    else
    {
      (enemy ? _enemyCards : _playerCards).Add(card.GetComponent<CharacterCard>());
    }
  }

  [ContextMenu("Ready")]
  public void Ready()
  {
    if (CheckForBattleEnd()) return;
    _playerCards[0].SetAnimation("ready");
    _enemyCards[0].SetAnimation("ready");
    AttackAndResolve();
  }

  [ContextMenu("AttackAndResolve")]
  public void AttackAndResolve()
  {
    StartCoroutine(Attack());
  }

  [ContextMenu("Attack")]
  IEnumerator Attack()
  {
    yield return new WaitForSeconds(3.0f);
    if (_playerCards.Count == 0 || _enemyCards.Count == 0)
    {
      Debug.Log("Attack-No more characters");
      yield break;
    }
    CharacterCard playerCharacter = _playerCards[0];
    CharacterCard enemyCharacter = _enemyCards[0];
    playerCharacter.SetAnimation("attack");
    enemyCharacter.SetAnimation("attack");
    yield return new WaitForSeconds(0.5f);
    _cameraShake.Shake();
    playerCharacter.Particles();
    enemyCharacter.Particles();
    yield return new WaitForSeconds(0.3f);

    playerCharacter.TakeDamage(enemyCharacter.Attack);
    enemyCharacter.TakeDamage(playerCharacter.Attack);

    // playerCharacter.Health -= enemyCharacter.Attack;
    // enemyCharacter.Health -= playerCharacter.Attack;



    if (playerCharacter.Health <= 0) playerCharacter.SetAnimation("die");
    if (enemyCharacter.Health <= 0) enemyCharacter.SetAnimation("die");
    yield return new WaitForSeconds(1.0f);
    Resolve();
  }

  [ContextMenu("Resolve")]
  public void Resolve()
  {
    Debug.Log("Resolve");
    CharacterCard playerCharacter = _playerCards[0];
    CharacterCard enemyCharacter = _enemyCards[0];
    if (playerCharacter.Health <= 0)
    {
      _playerCards.Remove(playerCharacter);
      playerCharacter.MinionDefeated();
      //Destroy(playerCharacter.gameObject);
    }
    if (enemyCharacter.Health <= 0)
    {
      _enemyCards.Remove(enemyCharacter);
      enemyCharacter.MinionDefeated();
      //Destroy(enemyCharacter.gameObject);
    }
    Debug.Log("Player: " + _playerCards.Count + " Enemy: " + _enemyCards.Count);



    // if (_playerCards.Count == 0 && _enemyCards.Count == 0)
    // {
    //   Debug.Log("DRAW!");
    //   OnBattleEnd?.Invoke("draw");
    //   return;
    // }


    // if (_playerCards.Count == 0)
    // {
    //   Debug.Log("You Lose");
    //   BattleData.Lives--;
    //   OnBattleEnd?.Invoke("lose");
    //   return;
    // }
    // if (_enemyCards.Count == 0)
    // {
    //   BattleData.Victories++;
    //   if (BattleData.Victories == 3)
    //   {
    //     Debug.Log("Victory");
    //     OnBattleEnd?.Invoke("victory");
    //   }
    //   else
    //   {
    //     Debug.Log("You Win");
    //     OnBattleEnd?.Invoke("win");
    //   }
    //   return;
    // }
    Ready();
  }

  bool CheckForBattleEnd()
  {
    if (battleEnded) return true;
    battleEnded = true;//Must fall through to return false
    if (_playerCards.Count == 0 && _enemyCards.Count == 0)
    {
      Debug.Log("DRAW!");
      OnBattleEnd?.Invoke("draw");
      return true;
    }
    if (_playerCards.Count == 0)
    {
      Debug.Log("You Lose");
      BattleData.Lives--;
      OnBattleEnd?.Invoke("lose");
      return true;
    }
    if (_enemyCards.Count == 0)
    {
      BattleData.Victories++;
      if (BattleData.Victories == 3)
      {
        Debug.Log("Victory");
        OnBattleEnd?.Invoke("victory");
      }
      else
      {
        Debug.Log("You Win");
        OnBattleEnd?.Invoke("win");
      }
      return true;
    }
    battleEnded = false;
    return false;
  }

}
