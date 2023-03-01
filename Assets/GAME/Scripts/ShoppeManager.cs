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
  public static Action<string> OnWalletAddressChanged;
  public static Action<bool> OnMinting;
  //public string wallet_state;
  public string wallet_id;

  private const string WALLETKEY = "wallet_address";
  private const string WALLETIDKEY = "wallet_id";
  // public string wallet_address;

  List<NFTItem> nftItems = new List<NFTItem>();
  [SerializeField] private GraphQLQuery Query;
  [SerializeField] private GraphQLQuery QueryCreateWallet;

  [SerializeField] private GraphQLQuery QueryWallet;

  [SerializeField] private GraphQLQuery QueryMintAndTransfer;

  [SerializeField] private GraphQLQuery QueryOwnedNFTs;


  [SerializeField] private GameData _gameData;


  void Start()
  {
    if (!PlayerPrefs.HasKey(WALLETKEY))
    {
      //PlayerPrefs.SetString("hero1unlocked", "");
      CreateWallet();
    }
    else
    {
      string address = PlayerPrefs.GetString(WALLETKEY);
      string id = PlayerPrefs.GetString(WALLETIDKEY);
      Debug.Log(id);
      OnWalletAddressChanged.Invoke(address);
      ExecuteQuery();
    }
  }

  public void ResultEvent(GraphQLResult result)
  {
    Dictionary<string, int> rarityOrder = new Dictionary<string, int>() { { "COMMON", 0 }, { "RARE", 1 }, { "LEGENDARY", 2 } };
    JObject data = JObject.Parse(result.Data.ToString());
    IList<JToken> results = data["nftModels"]["items"].Children().ToList();
    foreach (JToken jtoken in results)
    {
      NFTItem nftItem = jtoken.ToObject<NFTItem>();
      // Debug.Log("Data is here! " + nftItem.title);
      nftItem.image_url = jtoken["content"]["poster"]["url"].ToString();  //poster["poster"];
      nftItem.set_title = jtoken["set"]["title"].ToString();
      try
      {
        nftItem.orderby = rarityOrder[nftItem.rarity];
      }
      catch (System.Exception)
      {

      }

      nftItems.Add(nftItem);
    }
    _gameData.nftItems = nftItems.OrderBy(n => n.orderby).ToList();
    OnNFTItemsDownloaded?.Invoke();
    CheckForOwnedNFTs();
  }

  [ContextMenu("Execute Query")]
  public void ExecuteQuery()
  {
    Query.ExecuteQuery = true;
  }

  [ContextMenu("Create Wallet")]
  private void CreateWallet()
  {
    QueryCreateWallet.ExecuteQuery = true;
  }

  public void ResultEventCreateWallet(GraphQLResult result)
  {
    Debug.Log("CreateWallet:" + result.Data.ToString());
    JToken data = JToken.Parse(result.Data.ToString());
    wallet_id = data["createNiftoryWallet"]["id"].ToString();
    Debug.Log(wallet_id);
    QueryWallet.VariablesAsJson = "{'id':'" + wallet_id + "'}";
    QueryWallet.ExecuteQuery = true;
  }

  public void ResultEventQueryWallet(GraphQLResult result)
  {
    Debug.Log("CreateWallet:" + result.Data.ToString());
    JToken data = JToken.Parse(result.Data.ToString());
    string id = data["walletById"]["id"].ToString();
    string address = data["walletById"]["address"].ToString();
    string state = data["walletById"]["state"].ToString();
    Debug.Log("id: " + id + " address: " + address + " state:" + state);
    //wallet_state = state;
    if (state == "PENDING_CREATION")
    {
      StartCoroutine(QueryWalletAfterDelay(3.0f));
    }
    else
    {
      //Wallet is now created!
      PlayerPrefs.SetString(WALLETKEY, address);
      PlayerPrefs.SetString(WALLETIDKEY, id);
      //e6189dcb-80c4-411f-b451-383ca81a0557
      MintAndTransfer("e6189dcb-80c4-411f-b451-383ca81a0557");//Mint healer hero
      Debug.Log("Prefs: " + id);
      OnWalletAddressChanged.Invoke(address);
      ExecuteQuery();
    }
  }

  IEnumerator QueryWalletAfterDelay(float delay)
  {
    Debug.Log("QueryWalletAfterDelay");
    yield return new WaitForSeconds(delay);
    QueryWallet.VariablesAsJson = "{'id':'" + wallet_id + "'}";
    QueryWallet.ExecuteQuery = true;
    Debug.Log("QueryWalletAfterDelay2");
  }


  public void MintAndTransfer(string nftModelId)
  {
    OnMinting.Invoke(true);
    string address = PlayerPrefs.GetString(WALLETKEY);
    QueryMintAndTransfer.VariablesAsJson = "{'address':'" + address + "','nftModelId':'" + nftModelId + "'}";
    QueryMintAndTransfer.ExecuteQuery = true;
  }


  public void ResultEventQueryMintAndTransfer(GraphQLResult result)
  {
    Debug.Log("ResultEventQueryMintAndTransfer:" + result.Data.ToString());
    OnMinting.Invoke(false);
    CheckForOwnedNFTs();
  }


  [ContextMenu("CheckForOwnedNFTs")]
  public void CheckForOwnedNFTs()
  {
    string id = PlayerPrefs.GetString(WALLETIDKEY);
    QueryOwnedNFTs.VariablesAsJson = "{'id':'" + id + "'}";
    QueryOwnedNFTs.ExecuteQuery = true;
  }

  public void ResultEventQueryCheckForOwnedNFTs(GraphQLResult result)
  {
    Debug.Log("ResultEventQueryCheckForOwnedNFTs:" + result.Data.ToString());

    JObject data = JObject.Parse(result.Data.ToString());
    IList<JToken> results = data["walletById"]["nfts"].Children().ToList();
    foreach (JToken jtoken in results)
    {
      string modelId = jtoken["modelId"].ToString();
      Debug.Log(modelId);
      NFTItem nftItem = _gameData.nftItems.First(n => n.id == modelId);
      if (nftItem != null) nftItem.owned = true;
    }
    OnNFTItemsDownloaded?.Invoke();
  }


  [ContextMenu("ClearPrefs")]
  void ClearPrefs()
  {
    PlayerPrefs.DeleteAll();
  }

}
