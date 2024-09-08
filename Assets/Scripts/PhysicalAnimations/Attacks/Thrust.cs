using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/Thrust")]
public class Thrust : PhysAction //attack interface?? range, CanHit(), ...
{
    public int raiseFrames, thrustFrames, restFrames;
    public float range;
    public float footLean, feetBack;
    public override bool CanHit(Unit unit) //COPIED CHOP... This is: CANHIT STrAIGHT FWD From hand... where to put it...?
    {
        //raycast from hand forward
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(unit.bodyParts.rightHand.transform.position,
            unit.bodyParts.rightHand.transform.forward,
            out hit,
            range))
        {
            if (!hit.collider.gameObject.TryGetComponent<Unit>(out Unit targetEgg)) return false;
            if (targetEgg == unit) return false;
            Debug.Log("target right in front, attacking", targetEgg.gameObject);
            return true;
        }
        return false;
    }

    public override void Do(Unit unit, int currentFrame)
    {
        unit.physAnimator.StartAnimation(unit.stats.turn);
        var joint = unit.bodyParts.rightHandCJ;
        Vector3 position;
        int recoveryFrames = totalFrames - raiseFrames - thrustFrames;
        if (recoveryFrames <= 0) throw new System.Exception("animation is too short");

        if (currentFrame < raiseFrames) //Cock back
        {
            //hand position/rotation
            float progress = (float)(currentFrame + 1) / raiseFrames;
            position = Vector3.Lerp(unit.bodyStats.RightHandRestPos(), unit.bodyStats.RightHandBackPos(), progress);
            //Lean by tilting feet
            float desiredDegrees = Mathf.Lerp(0, footLean, progress);
            Quaternion footRot = Quaternion.Euler(desiredDegrees, 0, 0);
            ConfigurableJointExtensions.SetTargetRotationLocal(unit.bodyParts.leftFootCJ, footRot, unit.bodyParts.leftFootRot);
            ConfigurableJointExtensions.SetTargetRotationLocal(unit.bodyParts.rightFootCJ, footRot, unit.bodyParts.rightFootRot);
            //Lean by changing CA position of feet to be behind, reset at end of thrust
            float desiredFeetBack = Mathf.Lerp(0, feetBack, progress);
            unit.bodyParts.rightFootCJ.connectedAnchor = unit.bodyStats.RightFootRestPos() - new Vector3(0, 0, desiredFeetBack);
            unit.bodyParts.leftFootCJ.connectedAnchor = unit.bodyStats.LeftFootRestPos() - new Vector3(0, 0, desiredFeetBack);


        }
        else if (currentFrame < raiseFrames + thrustFrames) //Thrust, Back to FWD
        {
            if (currentFrame == raiseFrames)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(unit.bodyParts.leftFootCJ, unit.bodyParts.leftFootRot, unit.bodyParts.leftFootRot);
                ConfigurableJointExtensions.SetTargetRotationLocal(unit.bodyParts.rightFootCJ, unit.bodyParts.leftFootRot, unit.bodyParts.rightFootRot);
            }
            float progress = (float)(currentFrame - raiseFrames + 1) / thrustFrames;

            position = Vector3.Lerp(unit.bodyStats.RightHandBackPos(), unit.bodyStats.RightHandForwardPos(), progress);
        }
        else if (currentFrame < raiseFrames + thrustFrames + restFrames) //PAUSE FWD
        {
            if (currentFrame == raiseFrames + thrustFrames)
            {
                unit.bodyParts.leftFootCJ.connectedAnchor = unit.bodyStats.LeftFootRestPos();
                unit.bodyParts.rightFootCJ.connectedAnchor = unit.bodyStats.RightFootRestPos();
            }
            //float progress = (float)(currentFrame - raiseFrames - chopFrames + 1) / restFrames;
            position = unit.bodyStats.RightHandForwardPos();
        }
        else
        { //RECOVERY
            float progress = (float)(currentFrame - raiseFrames - thrustFrames - restFrames + 1) / recoveryFrames;
            position = Vector3.Lerp(unit.bodyStats.RightHandForwardPos(), unit.bodyStats.RightHandRestPos(), progress);
        }
        joint.connectedAnchor = position;
    }
}
