using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
  public static Action<int> OnManaUpdated;

  [SerializeField] private GameData _gameData;
  [SerializeField] private GameObject _cardPrefab;
  [SerializeField] private Transform _cardParent;
  [SerializeField] CardController _cardController;//Only needed for Slots!
  [SerializeField] Button _drawButton;

  private int _mana;

  private int mana
  {
    get { return _mana; }
    set
    {
      _mana = value;
      OnManaUpdated?.Invoke(_mana);
    }
  }

  void OnEnable()
  {
    CardController.OnCardPurchased += OnCardPurchased;
  }

  void OnDisable()
  {
    CardController.OnCardPurchased -= OnCardPurchased;
  }

  void OnCardPurchased(CharacterCard card)
  {
    mana -= 3;//card.Tier;
  }


  void Start()
  {
    mana = 10;
    Draw();
    mana = 10;
    // BattleData.Lives = 3;
    BattleData.Turns++;
    // BattleData.Victories = 0;
  }

  public void Draw()
  {
    if (_mana <= 0) return;
    StartCoroutine(AvoidDoubleClick());
    foreach (Transform child in _cardParent)
    {
      Destroy(child.gameObject);
    }
    mana--;
    List<Character> characters = _gameData.characters.Where(x => x.tier == 1).ToList();
    var shuffledList = characters.OrderBy(x => UnityEngine.Random.value).ToList();
    for (int i = 0; i < 3; i++)
    {
      Character character = shuffledList[i];
      GameObject card = Instantiate(_cardPrefab, _cardParent);
      card.GetComponent<CharacterCard>().SetCard(character);
    }
  }

  public void Battle()
  {
    BattleData._playerCharacters.Clear();
    BattleData._enemyCharacters.Clear();
    for (int i = 0; i < _cardController.Slots.Length; i++)
    {
      if (_cardController.Slots[i].characterCard == null) continue;
      string characterName = _cardController.Slots[i].characterCard.Name;
      Character character = _gameData.characters.Find(x => x.name == characterName);
      Character newCharacter = character.Clone() as Character;
      BattleData._playerCharacters.Add(newCharacter);
    }
    //Populate Enemy Characters here for now
    string[] names = new string[] { "Toad", "Bug", "Boar" };
    for (int i = 0; i < names.Length; i++)
    {
      Character character = _gameData.characters.Find(x => x.name == names[i]);
      Character newCharacter = character.Clone() as Character;
      BattleData._enemyCharacters.Add(newCharacter);
    }

    SceneManager.LoadScene("BattleScene");
  }


  IEnumerator AvoidDoubleClick()
  {
    _drawButton.interactable = false;
    yield return new WaitForSeconds(1);
    _drawButton.interactable = true;
  }


}
