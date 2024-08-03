using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement //limited to forces. Locomotion is a physanimation
{
    Unit egg;
    Footwork footwork;
    public Movement(Unit _egg)
    {
        egg = _egg;
        footwork = new Footwork(egg);
        //egg.StartCoroutine(ReportSpeed());
    }

    IEnumerator ReportSpeed()
    {
        float frequency = 2f;
        Vector3 position = egg.transform.position;
        while(true)
        {
            yield return Helpers.PauseForDuration(frequency);
            Debug.Log($"Average Speed over last {frequency} seconds: {Vector3.Distance(position, egg.transform.position)/frequency}");
            position = egg.transform.position;
        }
    }
    public void Process(Brain brain)
    {
        //Forces, Torques
        MoveForce(brain.move, ServiceLocator.Instance.soHolder.standardEggMoveStats.moveForce);
        TurnTorque(brain.look, 
            ServiceLocator.Instance.soHolder.standardEggMoveStats.turnP, 
            ServiceLocator.Instance.soHolder.standardEggMoveStats.turnD,
            ServiceLocator.Instance.soHolder.standardEggMoveStats.maxTorqueDeltaTheta);
        UprightForce(ServiceLocator.Instance.soHolder.standardEggMoveStats.uprightStrength);

        //Walk with feet (this contains nudgeForce...)
        //footwork.JointPositionWalkForward(brain.move);
    }

    public void TurnTorque(Vector2 lookInput, float turnP, float turnD, float maxTorqueDeltaTheta)
    {
        //PROPORTIONAL
        Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        Vector2 forwardV2 = new Vector2(flatForward.x, flatForward.z);
        float signedAngle = Vector2.SignedAngle(lookInput, forwardV2);
        if (Mathf.Abs(signedAngle) > maxTorqueDeltaTheta)
        {
            signedAngle /= Mathf.Abs(signedAngle);
            signedAngle *= maxTorqueDeltaTheta;
        }
        egg.bodyParts.rb.AddTorque(Vector3.up * signedAngle * turnP);
        //DERIVATIVE
        egg.bodyParts.rb.AddTorque(-egg.bodyParts.rb.angularVelocity * turnD);
        //INTEGRAL NEEDED? Spear will make it obvious if small deltas are problematic

    }
    public void UprightForce(float magnitude)
    {
        Vector3 headPosition = egg.transform.position + egg.transform.up;
        Vector3 footPosition = egg.transform.position - egg.transform.up;
        egg.bodyParts.rb.AddForceAtPosition(Vector3.up * magnitude, headPosition); 
        egg.bodyParts.rb.AddForceAtPosition(Vector3.down * magnitude, footPosition);
    }

    public void MoveForce(Vector2 moveInput, float moveStrength)
    {
        //Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        //Vector3 flatRight = Helpers.FlatForward(egg.transform.right);
        egg.bodyParts.rb.AddForce(Vector3.forward * moveInput.y * moveStrength);
        egg.bodyParts.rb.AddForce(Vector3.right * moveInput.x * moveStrength);
    }

    public void ForwardForce(float magnitude)
    {
        Vector3 flatForward = Helpers.FlatForward(egg.transform.forward);
        egg.bodyParts.rb.AddForce(flatForward * magnitude);
    }

    public void TurnRight(Unit egg, float magnitude)
    {
        egg.bodyParts.rb.AddTorque(Vector3.up * magnitude);
    }
}
