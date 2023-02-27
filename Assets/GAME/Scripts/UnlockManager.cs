using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
  [SerializeField] private Animator _animator;

  void Start()
  {
    if (!PlayerPrefs.HasKey("hero1unlocked"))
    {
      PlayerPrefs.SetInt("hero1unlocked", 1);
      _animator.SetTrigger("open");
    }
  }


  public void CloseUnlockScreen()
  {
    _animator.SetTrigger("close");
  }


  [ContextMenu("Clear All Prefs")]
  void ClearAllPrefs()
  {
    PlayerPrefs.DeleteAll();
  }

}
