using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition
{
    StateMachine sm;
    Func<Unit, bool> condition;
    State from, to;
    public Transition(StateMachine _sm, State _from, State _to, Func<Unit, bool> _condition)
    {
        sm = _sm;
        condition = _condition;
        from = _from;
        to = _to;
    }

    public bool Process()
    {
        if (!condition(sm.unit)) return false;
        //Debug.Log($"transitioning");
        from.TransitionTo(sm, to);
        return true;
    }
}
