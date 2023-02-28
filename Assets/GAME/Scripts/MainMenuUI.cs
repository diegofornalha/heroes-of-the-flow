using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

  public void LoadShop()
  {
    BattleData.Turns = 0;
    BattleData.Lives = 3;
    BattleData.Victories = 0;
    BattleData._playerCharacters.Clear();
    BattleData._enemyCharacters.Clear();

    UnityEngine.SceneManagement.SceneManager.LoadScene("ShopScene");
  }

  public void LoadShoppe()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("ShoppeScene");
  }


}
