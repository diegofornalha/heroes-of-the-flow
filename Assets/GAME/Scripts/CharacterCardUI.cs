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
  [SerializeField] private TextMeshProUGUI _damageText;

  [SerializeField] private Image _cardImage;
  [SerializeField] private Image _characterImage;
  [SerializeField] SpriteLibraryAsset _spriteLibrary;
  [SerializeField] private Image _tierImage;

  void OnEnable()
  {
    GetComponent<CharacterCard>().OnCharacterUpdated += SetCharacter;
    GetComponent<CharacterCard>().OnHealthUpdated += SetHealth;
    GetComponent<CharacterCard>().OnDamageTaken += damage => StartCoroutine(DamageText(damage));
  }

  void OnDisable()
  {
    GetComponent<CharacterCard>().OnCharacterUpdated -= SetCharacter;
    GetComponent<CharacterCard>().OnHealthUpdated -= SetHealth;
    GetComponent<CharacterCard>().OnDamageTaken -= damage => StartCoroutine(DamageText(damage));
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

  public int Tier { set => _tierImage.sprite = _spriteLibrary.GetSprite("tiers", value.ToString()); }


  private void SetCharacter(Character character)
  {
    _damageText.gameObject.SetActive(false);
    _nameText.text = character.name;
    _abilityText.text = character.ability;
    Tier = character.tier;
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

  private IEnumerator DamageText(int damage)
  {
    // Debug.Log("DamageText: " + damage);
    _damageText.text = damage.ToString();
    _damageText.gameObject.SetActive(true);
    yield return new WaitForSeconds(0.5f);
    _damageText.gameObject.SetActive(false);
  }


}
