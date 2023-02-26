using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
  public CharacterCardDrag currentCard;
  public UICardSlot currentSlot;
  [SerializeField] private UICardSlot[] _slots;
  [SerializeField] private Transform _placed;

  public UICardSlot[] Slots { get { return _slots; } }

  void OnEnable()
  {
    CharacterCardDrag.OnSelected += OnSelected;
    CharacterCardDrag.OnCardEndDrag += OnCardEndDrag;
  }

  void OnDisable()
  {
    CharacterCardDrag.OnSelected -= OnSelected;
    CharacterCardDrag.OnCardEndDrag -= OnCardEndDrag;
  }

  void OnSelected(CharacterCardDrag card)
  {
    currentCard = card;
  }

  void OnCardEndDrag(CharacterCardDrag card)
  {
    if (!currentCard) return;
    if (!currentSlot) return;
    currentCard.transform.position = currentSlot.transform.position;
    currentCard.GetComponent<CharacterCardUI>().IsCharacter = true;
    currentSlot.characterCard = currentCard.GetComponent<CharacterCard>();
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
