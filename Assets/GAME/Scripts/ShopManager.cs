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
    SacrificeController.OnCardSacrificed += OnCardSacrificed;
  }

  void OnDisable()
  {
    CardController.OnCardPurchased -= OnCardPurchased;
    SacrificeController.OnCardSacrificed -= OnCardSacrificed;
  }

  void OnCardPurchased(CharacterCard card)
  {
    mana -= 3;//card.Tier;
  }

  void OnCardSacrificed(CharacterCard card)
  {
    mana += 1;//card.Tier;
  }


  void Start()
  {
    BattleData.Turns++;
    mana = 10;
    Draw();
    mana = 10;
    StartCoroutine(PlaceExistingMinions());
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
    int tier = 1;
    if (BattleData.Turns == 3 || BattleData.Turns == 4) tier = 2;
    if (BattleData.Turns == 5 || BattleData.Turns == 6) tier = 3;
    List<Character> characters = _gameData.characters.Where(x => x.tier <= tier).ToList();
    // foreach (Character character in characters)
    // {
    //   Debug.Log(character.name);
    // }

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
    // string[] names = new string[] { "Toad", "Bug", "Boar" };
    // for (int i = 0; i < names.Length; i++)
    // {
    //   Character character = _gameData.characters.Find(x => x.name == names[i]);
    //   Character newCharacter = character.Clone() as Character;
    //   BattleData._enemyCharacters.Add(newCharacter);
    // }
    if (BattleData._playerCharacters.Count == 0) return;


    SceneManager.LoadScene("BattleScene");
  }


  IEnumerator AvoidDoubleClick()
  {
    _drawButton.interactable = false;
    yield return new WaitForSeconds(1);
    _drawButton.interactable = true;
  }


  [ContextMenu("Place Existing Minions")]
  IEnumerator PlaceExistingMinions()
  {
    yield return null;
    // string[] names = new string[] { "Spider", "Toad", "Bug" };
    // //_playerCharacters = new Character[names.Length];
    // for (int i = 0; i < names.Length; i++)
    // {
    //   Character character = _gameData.characters.Find(x => x.name == names[i]);
    //   Character newCharacter = character.Clone() as Character;
    //   BattleData._playerCharacters.Add(newCharacter);
    // }
    Debug.Log("PlaceExistingMinions:" + BattleData._playerCharacters.Count);
    for (int i = 0; i < BattleData._playerCharacters.Count; i++)
    {
      Character character = BattleData._playerCharacters[i];
      GameObject card = Instantiate(_cardPrefab, _cardController.Placed);
      CharacterCard characterCard = card.GetComponent<CharacterCard>();
      characterCard.SetCard(character);
      characterCard.IsCharacter = true;
      characterCard.transform.position = _cardController.Slots[i].transform.position;
      _cardController.Slots[i].characterCard = characterCard;
      characterCard.transform.position = _cardController.Slots[i].transform.position;
    }
  }


}
