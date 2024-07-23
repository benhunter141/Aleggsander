using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    Quaternion rot;
    Vector3 displacement;
    int smoothingFrames = 10;
    Vector3 avgTargetPosition;

    private void Awake()
    {
        rot = transform.rotation;
        displacement = transform.position - target.transform.position;
        avgTargetPosition = target.transform.position;
    }
    private void LateUpdate()
    {
        transform.rotation = rot;
        avgTargetPosition = (avgTargetPosition * smoothingFrames + target.transform.position) / (smoothingFrames + 1); 
        transform.position = avgTargetPosition + displacement;
    }
}
