using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadToDowndog : State
{
    Unit unit;
    PhysAction downwardDog;
    public QuadToDowndog(Unit _unit, PhysAction _downDog)
    {
        unit = _unit;
        downwardDog = _downDog;
    }
    public override void OnEnter()
    {
        Debug.Log("Quad to Down dog");
    }

    public override void Tick()
    {
        unit.physAnimator.StartAnimation(downwardDog);

    }
}
