using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ShoppeMenuUI : MonoBehaviour
{
  [SerializeField] private ShoppePanelUI _shoppePanelPrefab;
  [SerializeField] private GameData _gameData;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;
  [SerializeField] private Transform[] _panelParents;
  [SerializeField] private TextMeshProUGUI _walletAddressText;
  [SerializeField] private ShoppeManager _shoppeManager;

  [SerializeField] private GameObject _mintingPanel;

  private Dictionary<string, int> _shoppePanels = new Dictionary<string, int>() { { "Heroes", 0 }, { "Minion Hats", 1 }, { "Backgrounds", 2 } };

  void Awake()
  {
    foreach (Transform parent in _panelParents)
    {
      foreach (Transform child in parent)
      {
        Destroy(child.gameObject);
      }
    }
    _mintingPanel.SetActive(false);
  }


  void OnEnable()
  {
    ShoppeManager.OnNFTItemsDownloaded += CreatePanels;
    ShoppeManager.OnWalletAddressChanged += UpdateWalletAddress;
    ShoppeManager.OnMinting += HandleMinting;
  }

  void OnDisable()
  {
    ShoppeManager.OnNFTItemsDownloaded -= CreatePanels;
    ShoppeManager.OnWalletAddressChanged -= UpdateWalletAddress;
    ShoppeManager.OnMinting -= HandleMinting;
  }

  private void HandleMinting(bool minting)
  {
    _mintingPanel.SetActive(minting);
  }

  private void UpdateWalletAddress(string address)
  {
    _walletAddressText.text = "Flow wallet address: " + address;
  }


  public void LoadMainMenu()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
  }

  [ContextMenu("Create Panels")]
  void CreatePanels()
  {
    foreach (Transform parent in _panelParents)
    {
      foreach (Transform child in parent)
      {
        Destroy(child.gameObject);
      }
    }
    foreach (var item in _gameData.nftItems)
    {
      ShoppePanelUI shoppePanelUI = Instantiate(_shoppePanelPrefab, _panelParents[_shoppePanels[item.set_title]]);
      shoppePanelUI.SetItem(item);
      shoppePanelUI.SetImage(_spriteLibrary.GetSprite(item.set_title, item.title));
      shoppePanelUI.OnClick = (string nftId) =>
      {
        Debug.Log(nftId);
        _shoppeManager.MintAndTransfer(nftId);
      };
    }
  }


}
