using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/HighHandTorqueCircles")]
public class HighHandTorqueCircles : PhysAction
{
    public float radius;
    public override void Do(Unit unit, int currentFrame)
    {
        float progress = (float)currentFrame / totalFrames;
        //identify local axis of rotation needed

        //which hand is higher??
        float rightHeight = unit.transform.right.y;
        Joint highHand;
        Vector3 inwards;
        Vector3 restPos;
        if (rightHeight > 0)
        {
            highHand = unit.bodyParts.rightHandCJ;
            inwards = -Vector3.right;
            restPos = unit.bodyStats.RightHandRestPos();
        }
        else
        {
            highHand = unit.bodyParts.leftHandCJ;
            inwards = Vector3.right;
            restPos = unit.bodyStats.LeftHandRestPos();
        }
        //in either case, hand circles 'out' - inside, fwd, out, back
        Vector3 xDirection = inwards;
        Vector3 yDirection = Vector3.forward;
        Vector3 pos = Helpers.PositionOnCircle(progress * Mathf.PI * 2, radius, xDirection, yDirection);
        
        //set joint CA - LOCAL w respec to UNIT
        highHand.connectedAnchor = pos + restPos;
    }
}
