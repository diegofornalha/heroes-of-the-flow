using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

  public void LoadShop()
  {
    // Load the shop scene
    UnityEngine.SceneManagement.SceneManager.LoadScene("ShopScene");
  }

}
