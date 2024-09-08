using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erect : State
{
    Unit unit;
    PhysAction stand, tPose;

    public Erect(Unit _unit, PhysAction _stand, PhysAction _tPose)
    {
        unit = _unit;
        stand = _stand;
        tPose = _tPose;
    }
    public override void OnEnter()
    {
        Debug.Log("Now erect because was on heels, should try to balance and stand");
        unit.senses.WipeLeanHistory();
        //unit.physAnimator.StartAnimation(tPose);
    }

    public override void Tick()
    {
        unit.physAnimator.StartAnimation(stand);
    }
}
