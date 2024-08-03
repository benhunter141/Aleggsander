using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition
{
    Unit unit;
    Func<Unit, bool> condition;
    State from, to;
    public Transition(Unit _unit, State _from, State _to, Func<Unit, bool> _condition)
    {
        unit = _unit;
        condition = _condition;
        from = _from;
        to = _to;
    }

    public bool Process()
    {
        if (!condition(unit)) return false;
        //Debug.Log($"transitioning");
        from.TransitionTo(unit, to);
        return true;
    }
}
