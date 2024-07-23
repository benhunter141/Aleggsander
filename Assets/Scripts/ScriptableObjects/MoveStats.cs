using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/MoveStats")]
public class MoveStats : ScriptableObject
{
    public float moveForce;
    public float turnP, turnD, uprightStrength;

    //Step Metrics (NEW)
    public float footHeight, stepLength;
    public float footLateralDistance; //rest position for foot
    public float centreFwdHeight, centreBackHeight;
    public float centreBackLateralDistance; //tilted axis only for foot moving backwards
    public float maxConnectedAnchorDistance, maxTorqueDeltaTheta;
    //public float stepFirstPhase, stepSecondPhase;
    public float nudgeForce;
    public int stepDuration;


    //Most of the above are read from here, but some need to be updated if they are part of a component.

    //RIGIDBODY STATS
    public float mass, drag, angularDrag;
    public float footMass, footDrag, footAngularDrag;

    //CJ STATS
    public float spring, damper;

    //COLLIDER STATS
    public float bodyDynamicFriction, bodyStaticFriction, footDynamicFriction, footStaticFriction;

}
