using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Unit unit;
    public State currentState;

    public StateMachine(Unit _unit)
    {
        unit = _unit;
    }

}
