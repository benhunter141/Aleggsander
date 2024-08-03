using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public List<Transition> transitions = new List<Transition>();
    public abstract void Tick();
    public abstract void OnEnter();
    public void Transition()
    {
        //Debug.Log("1 checking transitions...");
        foreach (var t in transitions)
        {
            if (t.Process())
            {
                //Debug.Log("4 transition found and executing");
                break;
            }
        }
    }
    public void TransitionTo(Unit unit, State state)
    {
        //Debug.Log("2 setting current state");
        unit.currentState = state;
        //Debug.Log($"3 current state is now: {unit.currentState}");
        unit.currentState.OnEnter();
    }
}
