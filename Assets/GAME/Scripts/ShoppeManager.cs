using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQL4Unity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System;

public class ShoppeManager : MonoBehaviour
{
  public static Action OnNFTItemsDownloaded;

  List<NFTItem> nftItems = new List<NFTItem>();
  [SerializeField] private GraphQLQuery Query;
  [SerializeField] private GameData _gameData;

  void Start()
  {
    ExecuteQuery();
  }


  public void ResultEvent(GraphQLResult result)
  {
    JObject data = JObject.Parse(result.Data.ToString());
    IList<JToken> results = data["nftModels"]["items"].Children().ToList();
    foreach (JToken jtoken in results)
    {
      NFTItem nftItem = jtoken.ToObject<NFTItem>();
      Debug.Log("Data is here! " + nftItem.title);
      nftItem.image_url = jtoken["content"]["poster"]["url"].ToString();  //poster["poster"];
      nftItem.set_title = jtoken["set"]["title"].ToString();
      nftItems.Add(nftItem);
    }
    _gameData.nftItems = nftItems;
    OnNFTItemsDownloaded?.Invoke();
  }

  [ContextMenu("Execute Query")]
  public void ExecuteQuery()
  {
    Query.ExecuteQuery = true;
  }


}
