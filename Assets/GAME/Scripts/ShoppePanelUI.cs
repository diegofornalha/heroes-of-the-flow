using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShoppePanelUI : MonoBehaviour, IPointerClickHandler
{
  public Action<string> OnClick;
  public string nftId;
  [SerializeField] private TextMeshProUGUI _titleText;
  [SerializeField] private TextMeshProUGUI _rarityText;

  [SerializeField] private Image _itemImage;
  [SerializeField] private GameObject _tick;
  [SerializeField] private Color[] _colors;

  private Dictionary<string, int> rarityToColorIndex = new Dictionary<string, int>() { { "COMMON", 0 }, { "RARE", 1 }, { "LEGENDARY", 2 } };

  public void SetItem(NFTItem item)
  {
    nftId = item.id;
    _titleText.text = item.title;
    _rarityText.text = item.rarity;
    try
    {
      _rarityText.color = _colors[rarityToColorIndex[item.rarity]];
    }
    catch (System.Exception)
    {
    }
    _tick.SetActive(item.owned);
  }

  public void SetImage(Sprite sprite)
  {
    _itemImage.sprite = sprite;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!_tick.activeSelf) OnClick.Invoke(nftId);
  }
}
