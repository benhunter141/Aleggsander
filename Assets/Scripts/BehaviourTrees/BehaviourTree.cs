using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTree : ScriptableObject //behaviour trees set up state machines, which are ticked by the unit
{
    public abstract void Initialize(StateMachine sm);

}
