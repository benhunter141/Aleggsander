using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting
{
    Unit unit;
    int frames, retargetFrequency;
    Unit target;

    public Targeting(Unit _unit, int _retargetFrequency)
    {
        unit = _unit;
        frames = 0;
        retargetFrequency = _retargetFrequency;
    }
    public bool SelfTargeting(Unit _unit) => GetOldTarget() == unit; //sloppy and weird but it works...
    public Unit GetOldTarget() => target;
    public void RetargetTick()
    {
        if (frames % retargetFrequency == 0) target = GetNewTarget();
        unit.brain.ReceiveTarget(target.transform.position);
        frames++;
    }

    public Unit GetNewTarget() //improvement: consider facing direction so frontal targets are preferable
    {
        var allUnits = ServiceLocator.Instance.unitManager.allUnits;
        float distance = float.MaxValue;
        Unit target = unit;
        foreach(var u in allUnits)
        {
            if (u == unit) continue;
            if (u.allegiance == unit.allegiance) continue;
            float newDist = Vector3.Distance(u.transform.position, unit.transform.position);
            if (newDist >= distance) continue;
            distance = newDist;
            target = u;
        }
        if (target == unit)
        {
            //Debug.Log("targeting self, turn unit off or idle");
        }
        return target;
    }

}
