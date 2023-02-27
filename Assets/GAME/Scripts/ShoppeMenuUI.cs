using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ShoppeMenuUI : MonoBehaviour
{
  [SerializeField] private ShoppePanelUI _shoppePanelPrefab;
  [SerializeField] private GameData _gameData;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;
  [SerializeField] private Transform[] _panelParents;
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
  }


  void OnEnable()
  {
    ShoppeManager.OnNFTItemsDownloaded += CreatePanels;
  }

  void OnDisable()
  {
    ShoppeManager.OnNFTItemsDownloaded -= CreatePanels;
  }


  public void LoadMainMenu()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
  }

  [ContextMenu("Create Panels")]
  void CreatePanels()
  {

    foreach (var item in _gameData.nftItems)
    {
      ShoppePanelUI shoppePanelUI = Instantiate(_shoppePanelPrefab, _panelParents[_shoppePanels[item.set_title]]);
      shoppePanelUI.SetItem(item);
      shoppePanelUI.SetImage(_spriteLibrary.GetSprite(item.set_title, item.title));
    }
  }


}
