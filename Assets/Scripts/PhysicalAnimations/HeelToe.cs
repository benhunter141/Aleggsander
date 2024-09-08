using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PhysActions/HeelToe")]
public class HeelToe : PhysAction //over 100 frames, rotates toes down and back up to flat
{
    public float startRot, endRot;
    public override void Do(Unit egg, int currentFrame)
    {
        var rfcj = egg.bodyParts.rightFootCJ;
        var lfcj = egg.bodyParts.leftFootCJ;

        //get old rotations
        Quaternion rfOldRot = Quaternion.identity;
        Quaternion lfOldRot = Quaternion.identity;

        float progress = (float)currentFrame / totalFrames;
        float desiredDegrees = startRot + (endRot - startRot) * progress;

        //Debug.Log($"desiredDegrees:{desiredDegrees}");

        Quaternion desiredRot = Quaternion.Euler(desiredDegrees, 0, 0);

        ConfigurableJointExtensions.SetTargetRotationLocal(rfcj, desiredRot, rfOldRot);
        ConfigurableJointExtensions.SetTargetRotationLocal(lfcj, desiredRot, lfOldRot);
    }
}
