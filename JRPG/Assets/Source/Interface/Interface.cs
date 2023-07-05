using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField] private UnitActionPanel _unitActionPanel;
    [SerializeField] private GameObject _unitAttackButtons;

    private Unit _currentUnit;
    private Unit _targetUnit;

    private void Awake()
    {
        Unit.ChoisingUnit += OnChoisedUnit;
        Unit.ActionReady += ActiveInterface;
        Unit.DeathUnit += _unitActionPanel.RemoveInfoBlock;
    }

    private void OnDestroy()
    {
        Unit.ChoisingUnit -= OnChoisedUnit;
        Unit.ActionReady -= ActiveInterface;
        Unit.DeathUnit -= _unitActionPanel.RemoveInfoBlock;
    }

    private void Start()
    {
        foreach (var unit in Arena.Singleton.PlayerSquad.Units)
            _unitActionPanel.CreateInfoBlock(unit);

        foreach (var unit in Arena.Singleton.EnemySquad.Units)
            _unitActionPanel.CreateInfoBlock(unit);
    }

    private void ActiveInterface(Unit unit)
    {
        _unitActionPanel.UpdateActionTime();

        if (unit.Side == Unit.UnitSide.PLAYER)
        {
            _unitAttackButtons.SetActive(true);
            _currentUnit = unit;
            _currentUnit.UnitUI.ToggleTurnUnitArrow(true);

            if (_targetUnit != null)
                _targetUnit.UnitUI.ToggleChoisingUnitArrow(true);
        }
    }

    private void OnChoisedUnit(Unit unit)
    {
        if (_currentUnit == null)
            return;

        if (_targetUnit != null)
            _targetUnit.UnitUI.ToggleChoisingUnitArrow(false);

        _targetUnit = unit;
        _targetUnit.UnitUI.ToggleChoisingUnitArrow(true);
    }

    public void BaseAttack()
    {
        var currentUnit = _currentUnit;
        _currentUnit = null;

        if (_targetUnit == null)
            return;

        _unitAttackButtons.SetActive(false);
        currentUnit.BaseAttack(_targetUnit);

        currentUnit = null;
    }

    public void UseSkill()
    {
        var currentUnit = _currentUnit;
        _currentUnit = null;

        if (_targetUnit == null)
            return;

        _unitAttackButtons.SetActive(false);
        currentUnit.UseSkill(_targetUnit);
    }
}
