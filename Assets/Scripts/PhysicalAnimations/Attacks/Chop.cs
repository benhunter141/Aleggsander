using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/Chop")]
public class Chop : PhysAction
{
    public int raiseFrames, chopFrames, restFrames;
    public Vector3 rightHandRest, rightHandHigh, rightHandForward;
    public float footLean, feetBack;
    public float range;

    public override bool CanHit(Unit unit)
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
    public override void Do(Unit egg, int currentFrame)
    {
        //Cancel current walk animation in case of egg taking 120 frame steps
        //if(egg.physAnimator.AnimationCount() > 1) egg.physAnimator.ClearOtherAnimations(this);

        //right hand at rest, up high, forward
        //right hand rot identity, rocked back, rocked fwd

        //pos is the CA of the joint
        //rot is using extensionmethod

        var joint = egg.bodyParts.rightHandCJ;
        Vector3 position;
        Quaternion rotation;
        int recoveryFrames = totalFrames - raiseFrames - chopFrames;
        if (recoveryFrames <= 0) throw new System.Exception("animation is too short");

        Quaternion rest = Quaternion.identity;
        Quaternion back = Quaternion.Euler(-60, 0, 0);
        Quaternion fore = Quaternion.Euler(90, 0, 0);

        if (currentFrame < raiseFrames) //RAISE & Lean fwd
        {
            //hand position/rotation
            float progress = (float)(currentFrame + 1) / raiseFrames;
            position = Vector3.Lerp(rightHandRest, rightHandHigh, progress);
            rotation = Quaternion.Lerp(rest, back, progress);
            //Lean by tilting feet
            float desiredDegrees = Mathf.Lerp(0, footLean, progress);
            Quaternion footRot = Quaternion.Euler(desiredDegrees, 0, 0);
            ConfigurableJointExtensions.SetTargetRotationLocal(egg.bodyParts.leftFootCJ, footRot, egg.bodyParts.leftFootRot);
            ConfigurableJointExtensions.SetTargetRotationLocal(egg.bodyParts.rightFootCJ, footRot, egg.bodyParts.rightFootRot);
            //Lean by changing CA position of feet to be behind, reset at end of chop downstroke
            float desiredFeetBack = Mathf.Lerp(0, feetBack, progress);
            egg.bodyParts.rightFootCJ.connectedAnchor = egg.bodyStats.RightFootRestPos() - new Vector3(0, 0, desiredFeetBack);
            egg.bodyParts.leftFootCJ.connectedAnchor = egg.bodyStats.LeftFootRestPos() - new Vector3(0, 0, desiredFeetBack);


        }
        else if (currentFrame < raiseFrames + chopFrames) //CHOP
        {
            if(currentFrame == raiseFrames)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(egg.bodyParts.leftFootCJ, egg.bodyParts.leftFootRot, egg.bodyParts.leftFootRot);
                ConfigurableJointExtensions.SetTargetRotationLocal(egg.bodyParts.rightFootCJ, egg.bodyParts.leftFootRot, egg.bodyParts.rightFootRot);
            }
            float progress = (float)(currentFrame - raiseFrames + 1) / chopFrames;
            
            position = Helpers.Arc(rightHandHigh, rightHandForward, rightHandRest, progress);
            rotation = Quaternion.Lerp(back, fore, progress);
        }
        else if (currentFrame < raiseFrames + chopFrames + restFrames) //PAUSE FWD
        {
            if(currentFrame == raiseFrames + chopFrames)
            {
                egg.bodyParts.leftFootCJ.connectedAnchor = egg.bodyStats.LeftFootRestPos();
                egg.bodyParts.rightFootCJ.connectedAnchor = egg.bodyStats.RightFootRestPos();
            }
            //float progress = (float)(currentFrame - raiseFrames - chopFrames + 1) / restFrames;
            position = rightHandForward;
            rotation = fore;
        }
        else{ //RECOVERY
            float progress = (float)(currentFrame - raiseFrames - chopFrames - restFrames + 1) / recoveryFrames;
            position = Vector3.Lerp(rightHandForward, rightHandRest, progress);
            //rotation = Quaternion.Lerp(fore, rest, progress);
            rotation = Quaternion.identity;
        }
        joint.connectedAnchor = position;
        //Debug.Log($"rot:{rotation.w},{rotation.x},{rotation.y},{rotation.z}");
        ConfigurableJointExtensions.SetTargetRotationLocal(joint, rotation, egg.bodyParts.rightHandRot); //MAYBE YOU NEED THE CACHED STARTING ROT?
        //ConfigurableJointExtensions.SetTargetRotationLocal(joint, fore, Quaternion.identity);
    }
}
