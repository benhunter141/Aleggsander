using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/TPose")]
public class TPose : PhysAction //1 frame ready position used for transitions
{
    //USING CA Directly leads to super strength derpy behaviour
    //but how to reset CAs then...?

    //SHOULD BE REFACTORED TO USE 1 frame MUSCLES PUSH
    //But wait... if this is a 1 frame reset, it needs to set CA & spring and forget it, which is impossible because you need to keep calling muscles...
    //solution: muscles always handles fMax
    public override void Do(Unit unit, int currentFrame)
    {
        Debug.Log("T Pose just ran", unit.gameObject);
        //RESET HANDS POS AND ROT
        var rhcj = unit.bodyParts.rightHandCJ;
        var lhcj = unit.bodyParts.leftHandCJ;
        rhcj.connectedAnchor = unit.bodyStats.RightHandRestPos();
        lhcj.connectedAnchor = unit.bodyStats.LeftHandRestPos();
        ConfigurableJointExtensions.SetTargetRotationLocal(rhcj, Quaternion.identity, unit.bodyParts.rightHandRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(lhcj, Quaternion.identity, unit.bodyParts.leftHandRot);

        //FEET
        var rfcj = unit.bodyParts.rightFootCJ;
        var lfcj = unit.bodyParts.leftFootCJ;
        rfcj.connectedAnchor = unit.bodyStats.RightFootRestPos();
        lfcj.connectedAnchor = unit.bodyStats.LeftFootRestPos();
        ConfigurableJointExtensions.SetTargetRotationLocal(rfcj, Quaternion.identity, unit.bodyParts.rightFootRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(lfcj, Quaternion.identity, unit.bodyParts.leftFootRot);
    }
}
