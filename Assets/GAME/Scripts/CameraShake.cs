using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  public float duration = 0.5f;
  public float magnitude = 1.0f;
  public AnimationCurve curve;


  [ContextMenu("Shake")]
  public void Shake()
  {
    StartCoroutine(Shaking());
  }

  IEnumerator Shaking()
  {
    Vector3 startPosition = transform.position;
    float elapsedTime = 0f;
    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      float strength = curve.Evaluate(elapsedTime / duration);
      transform.position = startPosition + Random.insideUnitSphere * magnitude * strength;
      yield return null;
    }
    transform.position = startPosition;
  }

}
