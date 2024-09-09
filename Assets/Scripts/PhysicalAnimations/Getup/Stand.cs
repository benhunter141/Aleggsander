using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/Stand")]
public class Stand : PhysAction //stand and balance
{
    public float currentCAFactor, restCAFactor, leanCAFactor;
    public float lateralStabilityFactor; //make x value less significant. (0,1)
    public float handReachMultiplier, footReachMultiplier;
    public float toePointFactor;
    //public float reachMax;
    public override void Do(Unit unit, int currentFrame)
    {
        Vector3 leanDirection = unit.senses.LocalLeanPID();
        leanDirection = new Vector3(leanDirection.x * lateralStabilityFactor, 0, leanDirection.z);
        //Vector3 leanLocal = unit.transform.InverseTransformVector(leanDirection);

        Hands(unit, leanDirection);
        Feet(unit, leanDirection);
    }

    void Feet(Unit unit, Vector3 leanDirection)
    {
        //1. position the feet in the direction of lean so as to counteract falling
        //2. rotate feet to be... flat?
        Vector3 rfRest = unit.bodyStats.RightFootRestPos();
        Vector3 lfRest = unit.bodyStats.LeftFootRestPos();

        Vector3 rfCurrent = unit.bodyParts.rightFootCJ.connectedAnchor;
        Vector3 lfCurrent = unit.bodyParts.leftFootCJ.connectedAnchor;

        //feet go in direction of lean
        Vector3 rfLean = rfRest + leanDirection * footReachMultiplier;
        Vector3 lfLean = lfRest + leanDirection * footReachMultiplier;

        Vector3 smoothedRF = currentCAFactor * rfCurrent +
                            restCAFactor * rfRest +
                            leanCAFactor * rfLean;
        smoothedRF /= (currentCAFactor + restCAFactor + leanCAFactor);
        Vector3 smoothedLF = currentCAFactor * lfCurrent +
                            restCAFactor * lfRest +
                            leanCAFactor * lfLean;
        smoothedLF /= (currentCAFactor + restCAFactor + leanCAFactor);
        //draw lean direction on foot
        Vector3 leanGlobal = unit.senses.LeanPIDGlobal();
        //Debug.DrawRay(unit.bodyParts.rightFoot.transform.position, leanGlobal, Color.grey, 0.05f);
        //Debug.DrawRay(unit.bodyParts.leftFoot.transform.position, leanGlobal, Color.grey, 0.05f);
        unit.muscles.HardPushLimbTo(unit.bodyParts.rightFootCJ, smoothedRF, unit.bodyStats.footForceMax, unit.bodyStats.RightFootRestPos(), unit.bodyStats.legLength);
        unit.muscles.HardPushLimbTo(unit.bodyParts.leftFootCJ, smoothedLF, unit.bodyStats.footForceMax, unit.bodyStats.LeftFootRestPos(), unit.bodyStats.legLength);

        PointToes(unit, leanDirection);
    }

    void PointToes(Unit unit, Vector3 localLeanPID)
    {
        var rfcj = unit.bodyParts.rightFootCJ;
        var lfcj = unit.bodyParts.leftFootCJ;

        //get old rotations
        Quaternion rfOldRot = Quaternion.identity;
        Quaternion lfOldRot = Quaternion.identity;

        //Debug.Log($"desiredDegrees:{desiredDegrees}");
        float desiredDegrees = localLeanPID.z * toePointFactor;
        desiredDegrees = Mathf.Clamp(desiredDegrees, -75f, 75f);
        Quaternion desiredRot = Quaternion.Euler(desiredDegrees, 0, 0);

        ConfigurableJointExtensions.SetTargetRotationLocal(rfcj, desiredRot, rfOldRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(lfcj, desiredRot, lfOldRot);
    }

    void Hands(Unit unit, Vector3 leanDirection)
    {
        Vector3 rhRest = unit.bodyStats.RightHandRestPos();
        Vector3 lhRest = unit.bodyStats.LeftHandRestPos();

        Vector3 rhCurrent = unit.bodyParts.rightHandCJ.connectedAnchor;
        Vector3 lhCurrent = unit.bodyParts.leftHandCJ.connectedAnchor;

        //hands go opposite direction of lean
        Vector3 rhLean = rhRest - leanDirection * handReachMultiplier;
        Vector3 lhLean = lhRest - leanDirection * handReachMultiplier;

        //heavily weight current
        Vector3 smoothedRH = currentCAFactor * rhCurrent +
                             restCAFactor * rhRest +
                             leanCAFactor * rhLean;
        smoothedRH /= (currentCAFactor + restCAFactor + leanCAFactor);
        Vector3 smoothedLH = currentCAFactor * lhCurrent +
                              restCAFactor * lhRest +
                              leanCAFactor * lhLean;
        smoothedLH /= (currentCAFactor + restCAFactor + leanCAFactor);
        //show leanDir from hands
        //Vector3 leanGlobal = unit.senses.LeanPIDGlobal();
        //Debug.DrawRay(unit.bodyParts.rightHand.transform.position, leanGlobal, Color.grey, 0.05f);
        //Debug.DrawRay(unit.bodyParts.leftHand.transform.position, leanGlobal, Color.grey, 0.05f);

        unit.muscles.HardPushLimbTo(unit.bodyParts.rightHandCJ, smoothedRH, unit.bodyStats.handForceMax, unit.bodyStats.RightHandRestPos(), unit.bodyStats.armLength);
        unit.muscles.HardPushLimbTo(unit.bodyParts.leftHandCJ, smoothedLH, unit.bodyStats.handForceMax, unit.bodyStats.LeftHandRestPos(), unit.bodyStats.armLength);
    }
}
