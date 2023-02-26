using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICardSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public CharacterCard characterCard;
  [SerializeField] private Color __highlightColor;
  [SerializeField] private Image _spotlightImage;
  private Color _defaultColor;

  void Awake()
  {
    _defaultColor = _spotlightImage.color;
  }

  public bool Highlighted
  {
    set
    {
      if (value)
      {
        _spotlightImage.color = __highlightColor;
      }
      else
      {
        _spotlightImage.color = _defaultColor;
      }
    }
  }




  public void OnPointerEnter(PointerEventData eventData)
  {
    _spotlightImage.color = __highlightColor;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    _spotlightImage.color = _defaultColor;
  }
}
