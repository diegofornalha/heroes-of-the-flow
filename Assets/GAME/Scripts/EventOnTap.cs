using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventOnTap : MonoBehaviour, IPointerDownHandler
{
  public UnityEvent OnTap;
  public void OnPointerDown(PointerEventData eventData)
  {
    Debug.Log("Tapped");
    OnTap?.Invoke();
  }
}
