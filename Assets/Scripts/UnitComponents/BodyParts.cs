using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    //GOs
    public GameObject body, leftFoot, rightFoot, leftHand, rightHand;

    //Custom Components
    public DirectionRing directionRing;
    public RecruitSphere recruitSphere;

    //Gotten Components
    [HideInInspector]
    public ConfigurableJoint leftFootCJ, rightFootCJ, leftHandCJ, rightHandCJ;
    [HideInInspector]
    public Rigidbody rb, leftFootRB, rightFootRB, leftHandRB, rightHandRB;
    [HideInInspector]
    public Collider leftFootCollider, rightFootCollider, leftHandCollider, rightHandCollider, bodyCollider;
    [HideInInspector]
    public Quaternion leftFootRot, rightFootRot, leftHandRot, rightHandRot;

    protected virtual void Awake()
    {
        rb = body.GetComponent<Rigidbody>();
        bodyCollider = GetComponent<Collider>();
    }
}
