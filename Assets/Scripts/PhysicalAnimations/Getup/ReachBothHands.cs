using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/ReachBothHands")]
public class ReachBothHands : PhysAction
{
    public float reachDistance;
    public float smoothingRate;
    public override void Do(Unit unit, int currentFrame)
    {
        //which hand is higher?? Reach to low side
        float rightHeight = unit.transform.right.y;
        Vector3 lowDirection;
        Joint right = unit.bodyParts.rightHandCJ;
        Joint left = unit.bodyParts.leftHandCJ;
        Vector3 restRight = unit.bodyStats.RightHandRestPos();
        Vector3 restLeft = unit.bodyStats.LeftHandRestPos();
        if (rightHeight > 0)
        {
            lowDirection = -Vector3.right;
        }
        else
        {
            lowDirection = Vector3.right;
        }
        //set CA to smoothed position towards overhead
        Vector3 idealRightPosition = restRight + lowDirection * reachDistance;
        Vector3 idealLeftPosition = restLeft + lowDirection * reachDistance;

        Vector3 currentRightPosition = right.connectedAnchor;
        Vector3 currentLeftPosition = left.connectedAnchor;

        Vector3 smoothedRightPosition = (smoothingRate * currentRightPosition + idealRightPosition) / (smoothingRate + 1);
        Vector3 smoothedLeftPosition = (smoothingRate * currentLeftPosition + idealLeftPosition) / (smoothingRate + 1);

        right.connectedAnchor = smoothedRightPosition;
        left.connectedAnchor = smoothedLeftPosition;
    }
}
