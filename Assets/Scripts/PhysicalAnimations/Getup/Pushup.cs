using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActions/Pushup")]
public class Pushup : PhysAction
{
    public Vector3 rightHandLoadPosition, leftHandLoadPosition;
    public Vector3 rightHandPushPosition, leftHandPushPosition;
    public Vector3 rightFootPosition, leftFootPosition;
    public float toeAngle; //degrees the toe is pushing, torques should be refactored to muscles
    bool armsExtended;
    public override void Do(Unit unit, int currentFrame)
    {
        //position hands
        var rightHand = unit.bodyParts.rightHandCJ;
        var leftHand = unit.bodyParts.leftHandCJ;
        if(Rising(unit))
        {
            unit.muscles.HardPushLimbTo(rightHand, rightHandPushPosition, unit.bodyStats.handForceMax, unit.bodyStats.RightHandRestPos(), unit.bodyStats.armLength);
            unit.muscles.HardPushLimbTo(leftHand, leftHandPushPosition, unit.bodyStats.handForceMax, unit.bodyStats.LeftHandRestPos(), unit.bodyStats.armLength);
        }
        else if(unit.senses.Stable())
        {
            unit.muscles.HardPushLimbTo(rightHand, rightHandLoadPosition, unit.bodyStats.handForceMax, unit.bodyStats.RightHandRestPos(), unit.bodyStats.armLength);
            unit.muscles.HardPushLimbTo(leftHand, leftHandLoadPosition, unit.bodyStats.handForceMax, unit.bodyStats.LeftHandRestPos(), unit.bodyStats.armLength);
        }
        //dynamic foot angle, ideal torque is moderate from toes, ie. a lower angle when rising than this
        var rightFoot = unit.bodyParts.rightFootCJ;
        var leftFoot = unit.bodyParts.leftFootCJ;
        float footAngle = Vector3.SignedAngle(unit.transform.up, Vector3.up, unit.transform.right);
        ConfigurableJointExtensions.SetTargetRotationLocal(rightFoot, Quaternion.Euler(footAngle + toeAngle, 0, 0), unit.bodyParts.rightFootRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(leftFoot, Quaternion.Euler(footAngle + toeAngle, 0, 0), unit.bodyParts.leftFootRot);
        //foot position is left as it was in down dog
        unit.muscles.HardPushLimbTo(rightFoot, rightFootPosition, unit.bodyStats.footForceMax, unit.bodyStats.RightHandRestPos(), unit.bodyStats.armLength);
        unit.muscles.HardPushLimbTo(leftFoot, leftFootPosition, unit.bodyStats.footForceMax, unit.bodyStats.LeftHandRestPos(), unit.bodyStats.armLength);
    }

    bool Rising(Unit unit) => unit.senses.Rising();
     
    
}
