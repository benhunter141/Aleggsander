using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PhysActions/LowHandSideReach")]
public class LowHandSideReach : PhysAction
{
    public float overheadDistance; // try 3?
    public float smoothingRate; //1 is fast, 100 is slow ish
    public override void Do(Unit unit, int currentFrame)
    {
        //identify low hand joint
        float rightHeight = unit.transform.right.y;
        Joint lowHand;
        if (rightHeight > 0)
        {
            lowHand = unit.bodyParts.leftHandCJ;
        }
        else
        {
            lowHand = unit.bodyParts.rightHandCJ;
        }
        //set CA to smoothed position towards overhead
        Vector3 idealPosition = Vector3.up * overheadDistance;
        Vector3 currentPosition = lowHand.connectedAnchor;
        Vector3 smoothedPosition = (smoothingRate * currentPosition + idealPosition) / (smoothingRate + 1);
        lowHand.connectedAnchor = smoothedPosition;
    }
}
