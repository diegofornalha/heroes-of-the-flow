using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class WebInterface
{

  static async public Task<List<Character>> GetCharacters()
  {
    List<Character> characters = new List<Character>();
    string uri = "https://api.airtable.com/v0/apphfauTLydw7h0dE/tbl9gtLy9yYdKoTPZ/";
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      webRequest.SetRequestHeader("Authorization", "Bearer " + "pat8RoqOF0qMupcH5.f1838db7a6512fbc89adc36c3985d66bfef4377b2118804a28dd04194160f3b7");
      await webRequest.SendWebRequest();
      string metadataString = webRequest.downloadHandler.text;
      int count = 0;
      try
      {
        JObject data = JObject.Parse(metadataString);
        IList<JToken> results = data["records"].Children().ToList();
        // _characters.Clear();
        foreach (JToken result in results)
        {
          Character character = result["fields"].ToObject<Character>();
          // if (result["fields"]["data"] != null)
          // {
          //   Debug.Log("character.data: " + result["fields"]["data"].ToString());
          //   character.extradata = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["fields"]["data"].ToString());
          //   Debug.Log("character.extradata: " + character.extradata.Keys.Count);
          //   foreach (string key in character.extradata.Keys)
          //   {
          //     Debug.Log("key: " + key + " value: " + character.extradata[key]);
          //   }
          // }
          // character.cardImage = _spriteLibrary.GetSprite("cards", character.name);
          // character.characterImage = _spriteLibrary.GetSprite("characters", character.name);
          // if (character.cardImage == null && result["fields"]["url_image_card"] != null)
          // {
          //   character.cardImage = await GetImage(result["fields"]["url_image_card"].ToString());
          //   string path = Application.persistentDataPath + character.name + ".png";
          //   Debug.Log("path: " + path);
          //   File.WriteAllBytes(path, character.cardImage.texture.EncodeToPNG());
          // }
          // if (character.characterImage == null && result["fields"]["url_image_character"] != null)
          // {
          //   character.characterImage = await GetImage(result["fields"]["url_image_character"].ToString());
          // }
          // _characters.Add(character.name, character);
          characters.Add(character);
          count++;
          if (count > 0)
          {
            // break;
          }
        }
        // AssetDatabase.SaveAssets();
        return characters;
      }
      catch (System.Exception e)
      {
        Debug.Log("Error: " + e.Message);
        return null;
      }
    }
  }



  static async public Task<Sprite> GetImage(string imageURL)
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


  static Sprite SpriteFromTexture2D(Texture2D texture)
  {
    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
  }


}
