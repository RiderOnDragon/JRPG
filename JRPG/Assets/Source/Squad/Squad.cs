using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Squad : MonoBehaviour
{
    public enum SquadSide 
    { 
        PLAYER,
        ENEMY
    }


    [SerializeField] private Unit[] _unitsPrefab;
    [SerializeField] private SquadSide _side;

    private List<Unit> _units = new List<Unit>();

    public List<Unit> Units { get => _units; }

    public static event Action<Squad.SquadSide> AllUnitDefeated;

    private void Awake()
    {
        if (_side == SquadSide.PLAYER && _unitsPrefab.Length > 4)
            throw new Exception("The maximum number of units in a squad should not exceed 4");
        else if (_side == SquadSide.ENEMY && _unitsPrefab.Length > 5)
            throw new Exception("The maximum number of units in a squad should not exceed 5");

        Init();

        Unit.DeathUnit += OnDeathUnit;
    }

    private void OnDestroy()
    {
        Unit.DeathUnit -= OnDeathUnit;
    }

    private void Init()
    {
        for (int i = 0; i < _unitsPrefab.Length; i++)
        {
            var unit = Instantiate(_unitsPrefab[i]);
            unit.transform.SetParent(transform);

            if (_side == SquadSide.PLAYER)
            {
                unit.Side = Unit.UnitSide.PLAYER;
                unit.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
            else
            {
                unit.Side = Unit.UnitSide.ENEMY;

                unit.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }

            Vector3 position = Vector3.zero;
            position.z = 2 * i - (_unitsPrefab.Length - 1);
            unit.transform.localPosition = position;

            _units.Add(unit);
        }
    }

    private void OnDeathUnit(Unit unit)
    {
        _units.Remove(unit);

        if (_units.Count == 0)
            AllUnitDefeated?.Invoke(_side);
    }
}
