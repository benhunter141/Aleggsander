using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/DownwardDog")]
public class DownwardDog : PhysAction
{
    public Vector3 rightFootPosition, leftFootPosition, rightHandPosition, leftHandPosition;
    public float footShuffleAmplitude, footShufflePeriod, footAngle;
    public float handShuffleAmplitude, handShufflePeriod;
    public float smoothingRate;
    public override void Do(Unit unit, int currentFrame)
    {
        ConfigurableJoint rh = unit.bodyParts.rightHandCJ;
        ConfigurableJoint lh = unit.bodyParts.leftHandCJ;
        ConfigurableJoint rf = unit.bodyParts.rightFootCJ;
        ConfigurableJoint lf = unit.bodyParts.leftFootCJ;

        float handShuffleDelta = handShuffleAmplitude * Mathf.Sin(Time.time * 2 * Mathf.PI / handShufflePeriod);
        float footShuffleDelta = footShuffleAmplitude * Mathf.Sin(Time.time * 2 * Mathf.PI / footShufflePeriod);

        Vector3 idealRightHandPos = rightHandPosition + new Vector3(0, 0, handShuffleDelta);
        Vector3 idealLeftHandPos = leftHandPosition + new Vector3(0, 0, -handShuffleDelta);
        Vector3 idealRightFootPos = rightFootPosition + new Vector3(0, 0, footShuffleDelta);
        Vector3 idealLeftFootPos = leftFootPosition + new Vector3(0, 0, -footShuffleDelta);

        Vector3 currentRHpos = rh.connectedAnchor;
        Vector3 currentLHpos = lh.connectedAnchor;
        Vector3 currentRFpos = rf.connectedAnchor;
        Vector3 currentLFpos = lf.connectedAnchor;

        Vector3 rhSmoothed = (smoothingRate * currentRHpos + idealRightHandPos) / (smoothingRate + 1);
        Vector3 lhSmoothed = (smoothingRate * currentLHpos + idealLeftHandPos) / (smoothingRate + 1);
        Vector3 rfSmoothed = (smoothingRate * currentRFpos + idealRightFootPos) / (smoothingRate + 1);
        Vector3 lfSmoothed = (smoothingRate * currentLFpos + idealLeftFootPos) / (smoothingRate + 1);

        //don't use CA, this is old, use unit.muscles
        //rh.connectedAnchor = rhSmoothed;
        //lh.connectedAnchor = lhSmoothed;
        //rf.connectedAnchor = rfSmoothed;
        //lf.connectedAnchor = lfSmoothed;

        unit.muscles.HardPushLimbTo(rh, rhSmoothed, unit.bodyStats.handForceMax, unit.bodyStats.RightHandRestPos(), unit.bodyStats.armLength);
        unit.muscles.HardPushLimbTo(lh, lhSmoothed, unit.bodyStats.handForceMax, unit.bodyStats.LeftHandRestPos(), unit.bodyStats.armLength);
        unit.muscles.HardPushLimbTo(rf, rfSmoothed, unit.bodyStats.footForceMax, unit.bodyStats.RightFootRestPos(), unit.bodyStats.legLength);
        unit.muscles.HardPushLimbTo(lf, lfSmoothed, unit.bodyStats.footForceMax, unit.bodyStats.LeftFootRestPos(), unit.bodyStats.legLength);

        ConfigurableJointExtensions.SetTargetRotationLocal(rf, Quaternion.Euler(footAngle, 0, 0), unit.bodyParts.rightFootRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(lf, Quaternion.Euler(footAngle, 0, 0), unit.bodyParts.leftFootRot);
    }
}
