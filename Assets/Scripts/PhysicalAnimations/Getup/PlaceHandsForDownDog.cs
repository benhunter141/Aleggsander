using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/PlaceHandsForPushup")]
public class PlaceHandsForDownDog : PhysAction
{
    public int smoothingRate;
    public float reachDistance;
    public float footShuffleAmplitude, footShufflePeriod;
    public float footAngle;
    public Vector3 rightFootUnderButt, leftFootUnderButt;
    public override void Do(Unit unit, int currentFrame)
    {
        Joint rh = unit.bodyParts.rightHandCJ;
        Joint lh = unit.bodyParts.leftHandCJ;
        Joint rf = unit.bodyParts.rightFootCJ;
        Joint lf = unit.bodyParts.leftFootCJ;

        Vector3 idealRightHandPos = unit.bodyStats.RightHandRestPos() + Vector3.forward * reachDistance + Vector3.up * reachDistance;
        Vector3 idealLeftHandPos = unit.bodyStats.LeftHandRestPos() + Vector3.forward * reachDistance + Vector3.up * reachDistance;
        //Vector3 idealRightFootPos = unit.bodyStats.RightFootRestPos() + Vector3.forward * reachDistance;
        //Vector3 idealLeftFootPos = unit.bodyStats.LeftFootRestPos() + Vector3.forward * reachDistance;

        float rightFootShuffleDelta = footShuffleAmplitude * Mathf.Sin(2 * Mathf.PI * Time.time / footShufflePeriod);
        float leftFootShuffleDelta = -rightFootShuffleDelta;

        Vector3 idealRightFootPos = rightFootUnderButt + new Vector3(0,0, rightFootShuffleDelta);
        Vector3 idealLeftFootPos = leftFootUnderButt + new Vector3(0,0, leftFootShuffleDelta);

        Vector3 currentRHpos = rh.connectedAnchor;
        Vector3 currentLHpos = lh.connectedAnchor;
        Vector3 currentRFpos = rf.connectedAnchor;
        Vector3 currentLFpos = lf.connectedAnchor;

        Vector3 rhSmoothed = (smoothingRate * currentRHpos + idealRightHandPos) / (smoothingRate + 1);
        Vector3 lhSmoothed = (smoothingRate * currentLHpos + idealLeftHandPos) / (smoothingRate + 1);
        Vector3 rfSmoothed = (smoothingRate * currentRFpos + idealRightFootPos) / (smoothingRate + 1);
        Vector3 lfSmoothed = (smoothingRate * currentLFpos + idealLeftFootPos) / (smoothingRate + 1);

        //float rightFootShuffleDelta = 0;
        //float leftFootShuffleDelta = 0;

        rh.connectedAnchor = rhSmoothed;
        lh.connectedAnchor = lhSmoothed;
        rf.connectedAnchor = rfSmoothed;
        lf.connectedAnchor = lfSmoothed;

        ConfigurableJointExtensions.SetTargetRotationLocal((ConfigurableJoint)rf, Quaternion.Euler(footAngle, 0, 0), unit.bodyParts.rightFootRot);
        ConfigurableJointExtensions.SetTargetRotationLocal((ConfigurableJoint)lf, Quaternion.Euler(footAngle, 0, 0), unit.bodyParts.leftFootRot);
    }
}
