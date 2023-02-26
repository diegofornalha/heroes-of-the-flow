using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
  public static Action<CharacterCard> OnCardPurchased;
  public CharacterCardDrag currentCard;
  public UICardSlot currentSlot;
  [SerializeField] private UICardSlot[] _slots;
  [SerializeField] private Transform _placed;

  public UICardSlot[] Slots { get { return _slots; } }

  private int _mana;

  void OnEnable()
  {
    CharacterCardDrag.OnSelected += OnSelected;
    CharacterCardDrag.OnCardEndDrag += OnCardEndDrag;
    ShopManager.OnManaUpdated += UpdateMana;
  }

  void OnDisable()
  {
    CharacterCardDrag.OnSelected -= OnSelected;
    CharacterCardDrag.OnCardEndDrag -= OnCardEndDrag;
    ShopManager.OnManaUpdated -= UpdateMana;
  }

  void UpdateMana(int mana)
  {
    _mana = mana;
  }

  void OnSelected(CharacterCardDrag card)
  {
    currentCard = card;
  }

  void OnCardEndDrag(CharacterCardDrag card)
  {
    if (!currentCard) return;
    if (!currentSlot) return;
    if (currentSlot.characterCard != null) return;
    if (_mana < 3) return;//Not enough mana to purchase
    //Remove card from all other slots
    for (int i = 0; i < _slots.Length; i++)
    {
      if (_slots[i].characterCard == currentCard.GetComponent<CharacterCard>())
      {
        _slots[i].characterCard = null;
      }
    }

    currentCard.transform.position = currentSlot.transform.position;
    CharacterCard characterCard = currentCard.GetComponent<CharacterCard>();
    if (!characterCard.IsCharacter)
    { //Only wants to do this when moved from hand to board i.e. initially purchased
      characterCard.IsCharacter = true;
      OnCardPurchased?.Invoke(characterCard);
    }
    // currentCard.GetComponent<CharacterCardUI>().IsCharacter = true;
    currentSlot.characterCard = characterCard;
    currentCard.transform.SetParent(_placed);
    currentCard = null;
    currentSlot = null;
    for (int i = 0; i < _slots.Length; i++)
    {
      _slots[i].Highlighted = false;
    }
  }

  void Update()
  {
    if (!currentCard) return;
    //Highlight the slot that the card is over
    currentSlot = null;
    for (int i = 0; i < _slots.Length; i++)
    {
      _slots[i].GetComponent<UICardSlot>().Highlighted = false;
    }
    Vector3[] corners = new Vector3[4];
    currentCard.GetComponent<RectTransform>().GetWorldCorners(corners);
    Rect rect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    for (int i = 0; i < _slots.Length; i++)
    {
      _slots[i].GetComponent<RectTransform>().GetWorldCorners(corners);
      Rect slotRect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
      if (slotRect.Overlaps(rect))
      {
        _slots[i].GetComponent<UICardSlot>().Highlighted = true;
        currentSlot = _slots[i].GetComponent<UICardSlot>();
        return;
      }
    }
  }
}
