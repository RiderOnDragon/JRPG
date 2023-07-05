using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstUnit : Unit
{
    protected override IEnumerator UnitBaseAttack(Unit target)
    {
        _animation.Attack();
        target.TakeDamage(_damage);

        yield return new WaitForSecondsRealtime(1.5f);
    }

    protected override IEnumerator UnitUseSkill(Unit target)
    {
        _animation.Attack();
        target.TakeDamage(_damage * 2);

        yield return null;
    }
}
