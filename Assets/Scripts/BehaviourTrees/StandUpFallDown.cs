using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/BehaviourTrees/StandUpFallDown")]
public class StandUpFallDown : BehaviourTree
{
    State rollToBelly, bellyToQuad, quadToDownDog, downDogToErect, erect;
    public PhysAction canRoll, placeHands, downwardDog, pushup, stand, tPose;
    public float partlyOnBellyTolerance, fullyOnBellyTolerance, posturedUpTolerance;
    public float balancedTolerance;
    public override void Initialize(StateMachine sm)
    {
        //states
        rollToBelly = new RollToBelly(sm.unit, canRoll);
        bellyToQuad = new BellyToQuad(sm.unit, placeHands);
        quadToDownDog = new QuadToDowndog(sm.unit, downwardDog);
        downDogToErect = new DownDogToErect(sm.unit, pushup);
        erect = new Erect(sm.unit, stand, tPose);

        //transitions
        rollToBelly.transitions.Add(new Transition(sm, rollToBelly, bellyToQuad, BarelyOnBelly));
        bellyToQuad.transitions.Add(new Transition(sm, bellyToQuad, quadToDownDog, FullyOnBelly));
        bellyToQuad.transitions.Add(new Transition(sm, bellyToQuad, rollToBelly, NotBarelyOnBelly));
        quadToDownDog.transitions.Add(new Transition(sm, quadToDownDog, downDogToErect, PosturedUp));
        downDogToErect.transitions.Add(new Transition(sm, downDogToErect, erect, Balanced));
        erect.transitions.Add(new Transition(sm, erect, rollToBelly, Fallen));

        sm.currentState = erect;
    }

    //public bool BarelyOnBelly(Unit unit) => unit.senses.Stable() && unit.senses.OnBelly(90f);
    bool Fallen(Unit unit) => !unit.senses.Upright();
    bool OnHeels(Unit unit) => unit.senses.OnHeels();
    bool Balanced(Unit unit)
    {
        var local = unit.senses.LocalLeanPID();
        if (local.magnitude < balancedTolerance) return true;
        return false;
    }
    bool NotBarelyOnBelly(Unit unit) => !BarelyOnBelly(unit);
    bool PosturedUp(Unit unit)
    {
        float angle = Vector3.Angle(unit.transform.up, Vector3.up);
        if (angle < posturedUpTolerance) return true;
        return false;
    }
    bool BarelyOnBelly(Unit unit)
    {
        //if (!unit.senses.Stable()) return false;
        if (!unit.senses.OnBelly(partlyOnBellyTolerance)) return false;
        return true;
    }

    //public bool FullyOnBelly(Unit unit) => unit.senses.Stable() && unit.senses.OnBelly(10f);
    bool FullyOnBelly(Unit unit)
    {
        //if (!unit.senses.Stable()) return false;
        if (!unit.senses.OnBelly(fullyOnBellyTolerance)) return false;
        return true;
    }    
        
}
