using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterCardDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
  public static Action<CharacterCardDrag> OnSelected;
  public static Action<CharacterCardDrag> OnCardEndDrag;

  private RectTransform _rectTransform;
  // private Vector2 _offset;

  private Vector3 _defaultPosition;

  void Awake()
  {
    _rectTransform = GetComponent<RectTransform>();
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    StopAllCoroutines();
    _rectTransform.localScale = Vector3.one;
    //_offset = eventData.position - new Vector2(_rectTransform.position.x, _rectTransform.position.y);
    OnSelected?.Invoke(this);
  }

  public void OnDrag(PointerEventData eventData)
  {
    transform.position = eventData.position;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    //transform.localPosition = Vector3.zero;
    OnCardEndDrag?.Invoke(this);
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    _defaultPosition = transform.position;
    //OnSelected?.Invoke(this);
    StopAllCoroutines();
    //StartCoroutine(ScaleUp());
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    StopAllCoroutines();
    _rectTransform.localScale = Vector3.one;
    transform.position = _defaultPosition;
  }


  // IEnumerator ScaleUp()
  // {
  //   float scale = 1.2f;
  //   yield return new WaitForSeconds(0.5f);



  //   _rectTransform.localScale = Vector3.one * scale;

  //   transform.position = Input.mousePosition + new Vector3(0, _rectTransform.lossyScale.y * _rectTransform.rect.size.y / 2, 0);
  //   // while (_rectTransform.localScale.x < 1.2f)
  //   // {
  //   //   _rectTransform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
  //   //   yield return new WaitForSeconds(0.01f);
  //   // }
  // }

}
