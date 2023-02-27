using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppeMenuUI : MonoBehaviour
{
  public void LoadMainMenu()
  {
    // Load the shop scene
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
  }

}
