using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private GameObject _turnUnitArrow;
    [SerializeField] private GameObject _choisingUnitArrow;
    [SerializeField] private Slider HPBar;
    [SerializeField] private Sprite _avatar;

    public Sprite Avatar { get => _avatar; }

    public void UpdateHPBar(float maxHP, float currentHP)
    {
        if (currentHP <= 0)
            HPBar.gameObject.SetActive(false);

        float procentHP = currentHP / maxHP;
        HPBar.value = procentHP;
    }

    public void ToggleTurnUnitArrow(bool value)
    {
        _turnUnitArrow.SetActive(value);
    }

    public void ToggleChoisingUnitArrow(bool value)
    {
        _choisingUnitArrow.SetActive(value);
    }
}
