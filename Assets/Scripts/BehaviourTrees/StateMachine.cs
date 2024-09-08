using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Unit unit;
    public State currentState;
    public int framesInState;

    public StateMachine(Unit _unit)
    {
        unit = _unit;
    }

}
