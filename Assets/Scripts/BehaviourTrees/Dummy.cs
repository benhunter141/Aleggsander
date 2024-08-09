using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/BehaviourTrees/DummyTree")]
public class Dummy : BehaviourTree
{
    public override void Initialize(StateMachine sm)
    {
        //1 state, no transitions
        Sleep sleep = new Sleep(sm.unit);

        sm.currentState = sleep;
    }
}
