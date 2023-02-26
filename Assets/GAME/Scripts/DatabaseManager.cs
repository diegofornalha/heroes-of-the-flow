using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class Character : ICloneable
{
  public string name;
  public int tier;
  public int attack;
  public int health;
  public string ability;
  public string trigger;
  public string condition;
  public string effect;
  public string data;
  // public Sprite cardImage;
  // public Sprite characterImage;

  public object Clone()
  {
    return this.MemberwiseClone();
  }

}


public class DatabaseManager : MonoBehaviour
{
  [SerializeField] private GameData _gameData;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;
  [SerializeField] private GameObject _cardPrefab;
  [SerializeField] private Transform _cardParent;
  private Dictionary<string, Character> _characters = new Dictionary<string, Character>();

  public static DatabaseManager instance;
  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(this);
    }
  }

  [ContextMenu("Get Game Data")]
  async void GetGameData()
  {
    _gameData.characters = await WebInterface.GetCharacters();
    // foreach (Character character in _gameData.characters)
    // {
    //   character.cardImage = _spriteLibrary.GetSprite("cards", character.name);
    //   character.characterImage = _spriteLibrary.GetSprite("characters", character.name);
    // }
  }



  // async public Task GetCharacters()
  // {
  //   string uri = "https://api.airtable.com/v0/apphfauTLydw7h0dE/tbl9gtLy9yYdKoTPZ/";
  //   using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
  //   {
  //     webRequest.SetRequestHeader("Authorization", "Bearer " + "pat8RoqOF0qMupcH5.f1838db7a6512fbc89adc36c3985d66bfef4377b2118804a28dd04194160f3b7");
  //     await webRequest.SendWebRequest();
  //     string metadataString = webRequest.downloadHandler.text;
  //     try
  //     {
  //       JObject data = JObject.Parse(metadataString);
  //       IList<JToken> results = data["records"].Children().ToList();
  //       _characters.Clear();
  //       foreach (JToken result in results)
  //       {
  //         Character character = result["fields"].ToObject<Character>();
  //         //character.data = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["fields"]["url_image_card"].ToString());
  //         character.cardImage = _spriteLibrary.GetSprite("cards", character.name);
  //         character.characterImage = _spriteLibrary.GetSprite("characters", character.name);
  //         if (character.cardImage == null && result["fields"]["url_image_card"] != null)
  //         {
  //           character.cardImage = await GetImage(result["fields"]["url_image_card"].ToString());
  //         }
  //         if (character.characterImage == null && result["fields"]["url_image_character"] != null)
  //         {
  //           character.characterImage = await GetImage(result["fields"]["url_image_character"].ToString());
  //         }
  //         _characters.Add(character.name, character);
  //       }
  //     }
  //     catch (System.Exception e)
  //     {
  //       Debug.Log("Error: " + e.Message);
  //     }
  //   }
  // }

  Sprite SpriteFromTexture2D(Texture2D texture)
  {
    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
  }

  async public Task<Sprite> GetImage(string imageURL)
  {
    using (UnityWebRequest webRequest = UnityWebRequest.Get(imageURL))
    {
      await webRequest.SendWebRequest();
      if (webRequest.result != UnityWebRequest.Result.Success)
      {
        Debug.Log(webRequest.error);
        return null;
      }
      else
      {
        byte[] data = webRequest.downloadHandler.data;
        Debug.Log("data: " + data.Length);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data);
        Sprite sprite = SpriteFromTexture2D(texture as Texture2D);
        return sprite;
      }

    }
  }

  // async public Task<Sprite> GetImage(string imageURL)
  // {
  //   using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
  //   {
  //     await webRequest.SendWebRequest();
  //     if (webRequest.result != UnityWebRequest.Result.Success)
  //     {
  //       Debug.Log(webRequest.error);
  //       return null;
  //     }
  //     else
  //     {
  //       // Get downloaded asset bundle
  //       Texture texture = DownloadHandlerTexture.GetContent(webRequest);
  //       return SpriteFromTexture2D(texture as Texture2D);
  //     }
  //   }
  // }


  // public IEnumerator GetImage(string imageURL, Action<Texture> FinishDelegate)
  // {
  //   Debug.Log("GetImage: " + imageURL);
  //   using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
  //   {
  //     yield return webRequest.SendWebRequest();
  //     Texture texture = DownloadHandlerTexture.GetContent(webRequest);
  //     FinishDelegate(texture);
  //     // return SpriteFromTexture2D(texture as Texture2D);
  //   }
  // }

  // [ContextMenu("Test")]
  // async void StartNotYet()
  // {
  //   //StartCoroutine(GetImage("https://asset.cloudinary.com/morganpage/329d0a329eb93a193228740d6c28b3a5", (Texture tex) => { }));
  //   await GetCharacters();
  //   Vector2 pos = Vector3.zero;
  //   foreach (var character in _characters)
  //   {
  //     Debug.Log(character.Value.name + " " + character.Value.tier + " " + character.Value.attack + " " + character.Value.health);
  //     GameObject card = Instantiate(_cardPrefab, _cardParent);
  //     card.GetComponent<CharacterCard>().SetCard(character.Value);
  //     card.GetComponent<RectTransform>().anchoredPosition = pos;
  //     pos.x -= 100;
  //   }

  // }

  [ContextMenu("GameDataToCards")]
  void GameDataToCards()
  {
    Vector2 pos = Vector3.zero;
    foreach (var character in _gameData.characters)
    {
      Debug.Log(character.name + " " + character.tier + " " + character.attack + " " + character.health);
      GameObject card = Instantiate(_cardPrefab, _cardParent);
      card.GetComponent<CharacterCard>().SetCard(character);
      card.GetComponent<RectTransform>().anchoredPosition = pos;
      pos.x -= 100;
    }
  }


}
