using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DownloadManager : MonoBehaviour
{
  [SerializeField] private GameData _gameData;
  private async void Start()
  {
    await GetCharacter();
    await GetEnemies();
  }
  async Task GetCharacter()
  {
    Debug.Log("Getting characters");
    try
    {
      List<Character> characters = await WebInterface.GetCharacters();
      if (characters != null && characters.Count > 0)
      {
        _gameData.characters = characters;
      }
      else
      {
        Debug.Log("No characters found");
      }
    }
    catch (System.Exception ex)
    {
      Debug.Log(ex.Message);
    }
  }
  [ContextMenu("Get Enemies")]
  async Task GetEnemies()
  {
    Debug.Log("Getting enemies");
    try
    {
      List<Enemy> enemies = await WebInterface.GetEnemies();
      if (enemies != null && enemies.Count > 0)
      {
        _gameData.enemies = enemies;
      }
      else
      {
        Debug.Log("No enemies found");
      }
    }
    catch (System.Exception ex)
    {
      Debug.Log(ex.Message);
    }
  }


}
