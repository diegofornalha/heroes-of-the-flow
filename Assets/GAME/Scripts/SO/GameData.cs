using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class NFTItem
{
  public string id;
  public string set_title;
  public string title;
  public string description;
  public string image_url;

}

[System.Serializable]
public class Enemy
{
  public int turn;
  public int group;
  public string name;
}




[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
  public List<Character> characters = new List<Character>();

  public List<NFTItem> nftItems = new List<NFTItem>();

  public List<Enemy> enemies = new List<Enemy>();

}


