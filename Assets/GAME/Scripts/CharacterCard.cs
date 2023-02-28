using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
  public Action<Character> OnCharacterUpdated;
  public Action<int> OnHealthUpdated;
  public Action<int> OnDamageTaken;
  // private Action _onMinionDefeated;

  public static Action<Character, bool> SummonMinion;
  public static Action<int, bool> DamageRandomEnemy;

  public string Name;
  public int Tier;
  public int Attack;
  private int health;
  public bool IsEnemy;
  public string Trigger;
  public string Effect;
  private Dictionary<string, string> ExtraData;

  public bool IsCharacter
  {
    get => GetComponent<CharacterCardUI>().IsCharacter;
    set => GetComponent<CharacterCardUI>().IsCharacter = value;
  }

  public int Health
  {
    get => health;
    private set
    {
      health = value;
      OnHealthUpdated?.Invoke(health);
    }
  }

  public void TakeDamage(int damage)
  {
    Health -= damage;
    OnDamageTaken?.Invoke(damage);
  }


  public void SetCard(Character character)
  {
    GetComponent<CharacterCardUI>().IsCharacter = false;
    Name = character.name;
    Tier = character.tier;
    Attack = character.attack;
    Health = character.health;

    OnCharacterUpdated?.Invoke(character);
  }


  public void SetCharacter(Character character, bool enemy = false)
  {
    GetComponent<CharacterCardUI>().IsCharacter = true;
    GetComponent<CharacterCardUI>().Flip = enemy;
    IsEnemy = enemy;
    Name = character.name;
    Tier = character.tier;
    Attack = character.attack;
    Health = character.health;
    Trigger = character.trigger;
    Effect = character.effect;
    if (character.data != null)
    {
      try
      {
        ExtraData = JsonConvert.DeserializeObject<Dictionary<string, string>>(character.data);
      }
      catch (System.Exception ex)
      {
        Debug.Log(ex.Message);
      }
    }
    OnCharacterUpdated?.Invoke(character);
  }

  public void SetAnimation(string animation)
  {
    GetComponentInChildren<Animator>().SetTrigger(animation);
  }

  public void Particles()
  {
    GetComponentInChildren<ParticleSystem>().Play();
  }

  //Abilities
  public void StartOfBattle()
  {
    if (Trigger == "OnStart")
    {
      if (Effect == "Damage")
      {
        if (ExtraData["target"] == "enemy")
        {
          // DamageEnemy?.Invoke(int.Parse(ExtraData["amount"]));
          if (ExtraData["position"] == "random")
          {
            Debug.Log("Damage random enemy");
            // DamageRandomEnemy(1);
            DamageRandomEnemy?.Invoke(int.Parse(ExtraData["damage"]), IsEnemy);
          }
        }
        else if (ExtraData["target"] == "self")
        {
          // DamageSelf?.Invoke(int.Parse(ExtraData["amount"]));
        }
      }
    }
  }


  public void MinionDefeated() // Called when this minion is defeated
  {
    try
    {
      Debug.Log("Minion: " + Name + " died." + " Trigger: " + Trigger);
      //if (Trigger == null) return;
      // foreach (string key in ExtraData.Keys)
      // {
      //   Debug.Log("key: " + key + " value: " + ExtraData[key]);
      // }
      if (Trigger != null && Trigger == "OnDefeated")
      {
        if (Effect == "Summon")
        {
          Character character = new Character() { name = ExtraData["name"], attack = int.Parse(ExtraData["attack"]), health = int.Parse(ExtraData["health"]), ability = GetExtraData("ability") };
          SummonMinion?.Invoke(character, IsEnemy);
        }

      }
    }
    catch (System.Exception ex)
    {
      Debug.Log(ex.Message);
    }

    gameObject.SetActive(false);
    //_onMinionDefeated?.Invoke();
  }


  private string GetExtraData(string key)
  {
    try
    {
      return ExtraData[key];
    }
    catch (System.Exception)
    {
      Debug.Log("Key: " + key + " not found.");
      return "";
    }
  }


  // void OnDestroy()
  // {
  //   _onMinionDefeated?.Invoke();
  // }

}
