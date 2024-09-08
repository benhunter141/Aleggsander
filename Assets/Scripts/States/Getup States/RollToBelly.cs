using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollToBelly : State
{
    Unit unit;
    public float reachDistance, onBellyMinimumAngle;
    public bool onBellyFlag;
    PhysAction canRoll;

    //hands and feet over & under
    //lean slightly in low side direction
    //check if on belly, then flag for transition

    public RollToBelly(Unit _unit, PhysAction _canRoll)
    {
        unit = _unit;
        onBellyFlag = false;
        canRoll = _canRoll;
    }
    public override void OnEnter()
    {
        Debug.Log("Rolling to Belly");
    }

    public override void Tick()
    {
        unit.physAnimator.StartAnimation(canRoll);
        
    }
}
