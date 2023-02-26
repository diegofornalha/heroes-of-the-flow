using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SharedUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _manaText;
  [SerializeField] private TextMeshProUGUI _lifeText;
  [SerializeField] private TextMeshProUGUI _turnsText;
  [SerializeField] private TextMeshProUGUI _victoriesText;

  void OnEnable()
  {
    SetLife(BattleData.Lives);
    SetTurns(BattleData.Turns);
    SetVictories(BattleData.Victories);
    ShopManager.OnManaUpdated += SetMana;
    BattleData.OnLivesUpdated += SetLife;
    BattleData.OnTurnsUpdated += SetTurns;
    BattleData.OnVictoriesUpdated += SetVictories;
  }

  void OnDisable()
  {
    ShopManager.OnManaUpdated -= SetMana;
    BattleData.OnLivesUpdated -= SetLife;
    BattleData.OnTurnsUpdated -= SetTurns;
    BattleData.OnVictoriesUpdated -= SetVictories;
  }

  private void SetMana(int mana)
  {
    _manaText.text = mana.ToString();
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
