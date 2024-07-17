using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion
{
    EggController egg;
    public Locomotion(EggController _egg)
    {
        egg = _egg;
    }
    public void Process(Brain brain)
    {
        MoveForce(brain.move, ServiceLocator.Instance.soHolder.standardEggMoveStats.moveStrength);
        TurnTorque(brain.look, 
            ServiceLocator.Instance.soHolder.standardEggMoveStats.turnP, 
            ServiceLocator.Instance.soHolder.standardEggMoveStats.turnD);
        UprightForce(ServiceLocator.Instance.soHolder.standardEggMoveStats.uprightStrength);
    }

    public void TurnTorque(Vector2 lookInput, float turnP, float turnD)
    {
        //PROPORTIONAL
        Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        Vector2 forwardV2 = new Vector2(flatForward.x, flatForward.z);
        float signedAngle = Vector2.SignedAngle(lookInput, forwardV2);
        egg.rb.AddTorque(Vector3.up * signedAngle * turnP);
        //DERIVATIVE
        egg.rb.AddTorque(-egg.rb.angularVelocity * turnD);
        //INTEGRAL NEEDED?

    }
    public void UprightForce(float magnitude)
    {
        Vector3 headPosition = egg.transform.position + egg.transform.up;
        Vector3 footPosition = egg.transform.position - egg.transform.up;
        egg.rb.AddForceAtPosition(Vector3.up * magnitude, headPosition); 
        egg.rb.AddForceAtPosition(Vector3.down * magnitude, footPosition);
    }

    public void MoveForce(Vector2 moveInput, float moveStrength)
    {
        //Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        //Vector3 flatRight = Helpers.FlatForward(egg.transform.right);
        egg.rb.AddForce(Vector3.forward * moveInput.y * moveStrength);
        egg.rb.AddForce(Vector3.right * moveInput.x * moveStrength);
    }

    public void ForwardForce(float magnitude)
    {
        Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        egg.rb.AddForce(flatForward * magnitude);
    }

    public void TurnRight(EggController egg, float magnitude)
    {
        egg.rb.AddTorque(Vector3.up * magnitude);
    }
}
