using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BehaviourTrees/CommonMonster")]
public class CommonMonster : BehaviourTree
{
    State aggro, attackState, sleep;
    public override void Initialize(StateMachine sm) //must be called by unit
    {
        aggro = new Aggro(sm.unit);
        attackState = new AttackState(sm.unit);
        sleep = new Sleep(sm.unit);
        //STUN
        //DEATH
        //!!!!!!! best attack is stored !!!! need to use delegates to stored best attack ethod
        //THATS WHY transitions need Func bools and can't use regular ethods
        aggro.transitions.Add(new Transition(sm, aggro, attackState, sm.unit.CanHit));
        attackState.transitions.Add(new Transition(sm, attackState, aggro, sm.unit.physAnimator.IsIdling));
        aggro.transitions.Add(new Transition(sm, aggro, sleep, sm.unit.targeting.SelfTargeting));

        sm.currentState = aggro;
    }
}
