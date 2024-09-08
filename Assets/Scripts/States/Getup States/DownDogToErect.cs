using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDogToErect : State
{
    Unit unit;
    PhysAction pushup;

    public DownDogToErect(Unit _unit, PhysAction _pushup)
    {
        unit = _unit;
        pushup = _pushup;
    }
    public override void OnEnter()
    {
        Debug.Log("trying to push up from knees to erect");
    }

    public override void Tick()
    {
        unit.physAnimator.StartAnimation(pushup);
    }
}
