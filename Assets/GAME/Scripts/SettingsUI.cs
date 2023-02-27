using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
  [SerializeField] private GameObject _settingsPanel;

  void Start()
  {
    _settingsPanel.SetActive(false);
  }


  public void LoadMainMenu()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
  }

  public void Resume()
  {
    _settingsPanel.SetActive(false);
  }

  public void OpenSettings()
  {
    _settingsPanel.SetActive(true);
  }

}
