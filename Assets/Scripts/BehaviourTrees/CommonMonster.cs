using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BehaviourTrees/CommonMonster")]
public class CommonMonster : BehaviourTree
{
    State aggro, inMotion, sleep;
    public override void Initialize(StateMachine sm) //must be called by unit
    {
        aggro = new Aggro(sm.unit);
        inMotion = new AttackState(sm.unit);
        sleep = new Sleep(sm.unit);
        //STUN
        //DEATH

        aggro.transitions.Add(new Transition(sm, aggro, inMotion, sm.unit.stats.atk.CanHit));
        inMotion.transitions.Add(new Transition(sm, inMotion, aggro, sm.unit.physAnimator.IsIdling));
        aggro.transitions.Add(new Transition(sm, aggro, sleep, sm.unit.targeting.SelfTargeting));

        sm.currentState = aggro;
    }
}
