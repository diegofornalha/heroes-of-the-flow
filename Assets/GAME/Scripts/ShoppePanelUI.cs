using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShoppePanelUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _titleText;
  [SerializeField] private Image _itemImage;

  public void SetItem(NFTItem item)
  {
    _titleText.text = item.title;
  }

  public void SetImage(Sprite sprite)
  {
    _itemImage.sprite = sprite;
  }

}
