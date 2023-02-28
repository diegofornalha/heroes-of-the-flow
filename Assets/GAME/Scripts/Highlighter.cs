using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour
{
  [SerializeField] private Image _image;
  [SerializeField] private Color __highlightColor;
  private Color _defaultColor;

  void Awake()
  {
    _defaultColor = _image.color;
  }

  public bool Highlighted
  {
    set
    {
      if (value)
      {
        _image.color = __highlightColor;
      }
      else
      {
        _image.color = _defaultColor;
      }
    }
  }

}
