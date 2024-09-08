using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public List<Transition> transitions = new List<Transition>();
    public abstract void Tick();
    public abstract void OnEnter();
    public void CheckForTransitions(StateMachine sm)
    {
        sm.framesInState++;
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
    public void TransitionTo(StateMachine sm, State state)
    {
        //Debug.Log("2 setting current state");
        sm.currentState = state;
        //Debug.Log($"3 current state is now: {unit.currentState}");
        sm.currentState.OnEnter();
        sm.framesInState = 0;
    }
}
