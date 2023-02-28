using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _outcomeText;
  [SerializeField] private Image _image;

  [SerializeField] private TextMeshProUGUI _lifeText;
  [SerializeField] private TextMeshProUGUI _turnsText;
  [SerializeField] private TextMeshProUGUI _victoriesText;
  [SerializeField] private Animator _animator;

  [SerializeField] private Sprite[] _sprites;
  [SerializeField] private Color[] _colors;

  void OnEnable()
  {
    BattleData.OnLivesUpdated += SetLife;
    BattleData.OnTurnsUpdated += SetTurns;
    BattleData.OnVictoriesUpdated += SetVictories;
    BattleManager.OnBattleEnd += HandleBattleEnd;
  }

  void OnDisable()
  {
    BattleData.OnLivesUpdated -= SetLife;
    BattleData.OnTurnsUpdated -= SetTurns;
    BattleData.OnVictoriesUpdated -= SetVictories;
    BattleManager.OnBattleEnd -= HandleBattleEnd;
  }

  private void HandleBattleEnd(string outcome)
  {
    Debug.Log("Battle ended with outcome: " + outcome);
    SetLife(BattleData.Lives);
    SetTurns(BattleData.Turns);
    SetVictories(BattleData.Victories);
    if (outcome == "draw")
    {
      _outcomeText.text = "DRAW";
      _outcomeText.color = _colors[0];
      _image.sprite = _sprites[0];
    }
    else if (outcome == "win")
    {
      _outcomeText.text = "BATTLE WON";
      _outcomeText.color = _colors[1];
      _image.sprite = _sprites[1];
    }
    else if (outcome == "lose")
    {
      _outcomeText.text = "BATTLE LOST";
      _outcomeText.color = _colors[2];
      _image.sprite = _sprites[2];
    }
    else if (outcome == "victory")
    {
      _outcomeText.text = "VICTORY!!!";
      _outcomeText.color = _colors[1];
      _image.sprite = _sprites[3];
    }
    else
    {
      _outcomeText.text = outcome;
    }
    _animator.SetTrigger("open");//Open gameover panel
  }

  public void LoadShop()
  {
    if (_outcomeText.text == "VICTORY!!!" || BattleData.Turns >= 10 || BattleData.Lives <= 0)
    {
      SceneManager.LoadScene("MainScene");//Main scene resets all data so load this when victory achieved
    }
    else
    {
      SceneManager.LoadScene("ShopScene");
    }

  }


  private void SetLife(int life)
  {
    _lifeText.text = life.ToString();
  }

  private void SetTurns(int turns)
  {
    _turnsText.text = turns.ToString();
  }

  private void SetVictories(int victories)
  {
    _victoriesText.text = victories.ToString();
  }


}
