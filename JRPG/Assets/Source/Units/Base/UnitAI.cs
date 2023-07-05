using System.Collections;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    private Squad _playerSquad;
    private Squad _enemySquad;

    private void Awake()
    {
        _playerSquad = FindObjectOfType<PlayerSquad>();
        _enemySquad = FindObjectOfType<EnemySquad>();
    }

    public Coroutine AIAttack(Unit unit)
    {
        return StartCoroutine(Attack(unit));
    }

    private IEnumerator Attack(Unit unit)
    {
        yield return new WaitForSecondsRealtime(1.5f);

        var squad = unit.Side == Unit.UnitSide.PLAYER ? _enemySquad : _playerSquad;

        if (squad.Units.Count == 0)
            yield return null;

        int index = Random.Range(0, squad.Units.Count);
        var target = _playerSquad.Units[index];

        unit.BaseAttack(target);
    }
}
