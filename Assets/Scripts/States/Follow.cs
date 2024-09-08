using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : State
{
    Unit unit;
    PhysAction walk, chop;
    public Follow(Unit _unit)
    {
        unit = _unit;
        walk = ServiceLocator.Instance.animations.walk;
        chop = ServiceLocator.Instance.animations.chop;
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        //Debug.Log($"follow tick", egg.gameObject);
        if (!unit.physAnimator.IsWalking()) unit.physAnimator.StartAnimation(walk);
        if (!unit.physAnimator.IsChopping() && chop.CanHit(unit)) unit.physAnimator.StartAnimation(chop);
        CheckForTransitions(unit.sm);
    }
}
