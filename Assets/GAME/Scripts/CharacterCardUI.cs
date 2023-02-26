using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class CharacterCardUI : MonoBehaviour
{
  [SerializeField] private GameObject _card;
  [SerializeField] private GameObject _character;

  [SerializeField] private TextMeshProUGUI _nameText;
  [SerializeField] private TextMeshProUGUI _abilityText;

  [SerializeField] private TextMeshProUGUI[] _attackText;
  [SerializeField] private TextMeshProUGUI[] _healthText;

  [SerializeField] private Image _cardImage;
  [SerializeField] private Image _characterImage;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;

  void OnEnable()
  {
    GetComponent<CharacterCard>().OnCharacterUpdated += SetCharacter;
    GetComponent<CharacterCard>().OnHealthUpdated += SetHealth;
  }

  void OnDisable()
  {
    GetComponent<CharacterCard>().OnCharacterUpdated -= SetCharacter;
    GetComponent<CharacterCard>().OnHealthUpdated -= SetHealth;
  }



  public bool IsCharacter
  {
    get => _character.activeSelf;
    set
    {
      _card.SetActive(!value);
      _character.SetActive(value);
    }
  }

  public bool Flip
  {
    set
    {
      _characterImage.GetComponent<RectTransform>().parent.parent.localScale = new Vector3(value ? -1 : 1, 1, 1);
    }
  }

  private void SetCharacter(Character character)
  {
    _nameText.text = character.name;
    _abilityText.text = character.ability;
    _cardImage.sprite = _spriteLibrary.GetSprite("cards", character.name);
    _characterImage.sprite = _spriteLibrary.GetSprite("characters", character.name);
    // _cardImage.sprite = character.cardImage;
    // _characterImage.sprite = character.characterImage;
    foreach (TextMeshProUGUI attackText in _attackText)
    {
      attackText.text = character.attack.ToString();
    }
    foreach (TextMeshProUGUI healthText in _healthText)
    {
      healthText.text = character.health.ToString();
    }
  }

  private void SetHealth(int health)
  {
    foreach (TextMeshProUGUI healthText in _healthText)
    {
      healthText.text = health.ToString();
    }
  }


}
