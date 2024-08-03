using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : State
{
    Unit unit;
    public Sleep(Unit _unit)
    {
        unit = _unit;
    }
    public override void OnEnter()
    {
        Debug.Log("sleep", unit.gameObject);
        //do nothing
    }

    public override void Tick()
    {
        //do nothing
    }
}
