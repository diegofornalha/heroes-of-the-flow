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

}
