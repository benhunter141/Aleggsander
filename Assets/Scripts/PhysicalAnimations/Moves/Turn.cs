using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/Turn")]
public class Turn : PhysAction
{
    public float maxTorqueDeltaTheta, turnP, turnD;
    public override void Do(Unit unit, int currentFrame)
    {
        unit.brain.ReceiveTarget(unit.targeting.GetNewTarget().transform.position);
                    //PROPORTIONAL
        Vector3 flatForward = Helpers.FlatForward(unit.transform.forward);
        Vector2 forwardV2 = new Vector2(flatForward.x, flatForward.z);
        float signedAngle = Vector2.SignedAngle(unit.brain.look, forwardV2);
        if (Mathf.Abs(signedAngle) > maxTorqueDeltaTheta)
        {
            signedAngle /= Mathf.Abs(signedAngle);
            signedAngle *= maxTorqueDeltaTheta;
        }
        unit.bodyParts.rb.AddTorque(Vector3.up * signedAngle * turnP);
                    //DERIVATIVE
        unit.bodyParts.rb.AddTorque(-unit.bodyParts.rb.angularVelocity * turnD);
                    //INTEGRAL NEEDED?

    }
}
