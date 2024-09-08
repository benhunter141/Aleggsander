using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State //this state clears PA and starts attack, transitions when ... (?) atk PA ends? see behaviour tree
{
    Unit unit;

    public AttackState(Unit _unit)
    {
        unit = _unit;
        transitions = new List<Transition>();
    }
    public override void OnEnter()
    {
        unit.physAnimator.ClearAllAnimations();
        PhysAction bestAttack = unit.BestAttack();
        unit.physAnimator.StartAnimation(bestAttack);
    }

    public override void Tick()
    {
        CheckForTransitions(unit.sm);
    }

    //bool Attacking() => unit.physAnimator.IsAnimating(unit.stats.atk);
    //void Attack() => unit.physAnimator.StartAnimation(unit.stats.atk);
}
