using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/CanRoll")]
public class CanRoll : PhysAction
{
    public float reachDistance;
    public int smoothingRate;
    public override void Do(Unit unit, int currentFrame)
    {
        //which hand is higher?? Reach to low side
        float rightHeight = unit.transform.right.y;
        Vector3 lowDirection;
        if (rightHeight > 0)
        {
            lowDirection = -Vector3.right;
        }
        else
        {
            lowDirection = Vector3.right;
        }
        //Hands
        Joint rightHand = unit.bodyParts.rightHandCJ;
        Joint leftHand = unit.bodyParts.leftHandCJ;
        Vector3 highRight = unit.bodyStats.RightHandOverheadPos();
        Vector3 highLeft = unit.bodyStats.LeftHandOverheadPos();
        //set CA to smoothed position towards overhead
        Vector3 idealRightPosition = highRight + lowDirection * reachDistance;
        Vector3 idealLeftPosition = highLeft + lowDirection * reachDistance;

        Vector3 currentRightPosition = rightHand.connectedAnchor;
        Vector3 currentLeftPosition = leftHand.connectedAnchor;

        Vector3 smoothedRightPosition = (smoothingRate * currentRightPosition + idealRightPosition) / (smoothingRate + 1);
        Vector3 smoothedLeftPosition = (smoothingRate * currentLeftPosition + idealLeftPosition) / (smoothingRate + 1);

        rightHand.connectedAnchor = smoothedRightPosition;
        leftHand.connectedAnchor = smoothedLeftPosition;

        //Feet also, but little bit fwd
        Joint rightFoot = unit.bodyParts.rightFootCJ;
        Joint leftFoot = unit.bodyParts.leftFootCJ;

        Vector3 idealRightFootPosition = unit.bodyStats.RightUnderFootPos() + Vector3.forward * reachDistance;
        Vector3 idealLeftFootPosition = unit.bodyStats.LeftUnderFootPos() + Vector3.forward * reachDistance;

        Vector3 currentRightFoot = rightFoot.connectedAnchor;
        Vector3 currentLeftFoot = leftFoot.connectedAnchor;

        Vector3 smoothedRightFoot = (smoothingRate * currentRightFoot + idealRightFootPosition) / (smoothingRate + 1);
        Vector3 smoothedLeftFoot = (smoothingRate * currentLeftFoot + idealLeftFootPosition) / (smoothingRate + 1);

        rightFoot.connectedAnchor = smoothedRightFoot;
        leftFoot.connectedAnchor = smoothedLeftFoot;
    }


}
