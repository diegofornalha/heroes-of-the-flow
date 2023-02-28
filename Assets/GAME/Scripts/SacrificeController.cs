using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SacrificeController : MonoBehaviour
{
  public static Action<CharacterCard> OnCardSacrificed;
  public CharacterCardDrag currentCard;
  public bool Sacrifice = false;
  [SerializeField] private GameObject _sacrifice;
  [SerializeField] private CardController _cardController;

  void OnEnable()
  {
    CharacterCardDrag.OnCardEndDrag += OnCardEndDrag;
    CharacterCardDrag.OnSelected += OnSelected;
  }

  void OnDisable()
  {
    CharacterCardDrag.OnCardEndDrag -= OnCardEndDrag;
    CharacterCardDrag.OnSelected -= OnSelected;
  }


  void OnCardEndDrag(CharacterCardDrag characterCardDrag)
  {
    CharacterCard characterCard = characterCardDrag.GetComponent<CharacterCard>();
    if (!characterCard.IsCharacter) return; //Only interested in characters

    if (Sacrifice)
    {
      _cardController.RemoveCardFromSlots(characterCard);
      characterCard.gameObject.SetActive(false);
      OnCardSacrificed?.Invoke(characterCard);
    }

  }

  void OnSelected(CharacterCardDrag card)
  {
    currentCard = card;
  }
  // Update is called once per frame
  void Update()
  {
    if (!currentCard) return;
    _sacrifice.GetComponent<Highlighter>().Highlighted = false;
    Sacrifice = false;
    CharacterCard characterCard = currentCard.GetComponent<CharacterCard>();
    if (!characterCard.IsCharacter) return; //Only interested in characters



    Vector3[] corners = new Vector3[4];
    currentCard.GetComponent<RectTransform>().GetWorldCorners(corners);
    Rect rect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    _sacrifice.GetComponent<RectTransform>().GetWorldCorners(corners);
    Rect slotRect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    if (slotRect.Overlaps(rect))
    {
      _sacrifice.GetComponent<Highlighter>().Highlighted = true;
      Sacrifice = true;
    }
  }

}
