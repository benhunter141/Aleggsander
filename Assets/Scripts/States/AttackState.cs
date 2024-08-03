using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State //this state basically does nothing, waits until animation finishes then transitions usually, defined in Slime.cs
{
    Unit unit;

    public AttackState(Unit _unit)
    {
        unit = _unit;
        transitions = new List<Transition>();
    }
    public override void OnEnter()
    {
        unit.physAnimator.StartAnimation(unit.stats.atk);
    }

    public override void Tick()
    {
        Transition();
    }

    //bool Attacking() => unit.physAnimator.IsAnimating(unit.stats.atk);
    //void Attack() => unit.physAnimator.StartAnimation(unit.stats.atk);
}
