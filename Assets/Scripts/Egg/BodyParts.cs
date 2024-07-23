using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    public GameObject body, leftFoot, rightFoot, leftHand, rightHand;
    [HideInInspector]
    public ConfigurableJoint leftFootCJ, rightFootCJ, leftHandCJ, rightHandCJ;
    [HideInInspector]
    public Rigidbody rb, leftFootRB, rightFootRB, leftHandRB, rightHandRB;
    [HideInInspector]
    public Collider leftFootCollider, rightFootCollider, leftHandCollider, rightHandCollider, bodyCollider;

    private void Awake()
    {
        leftFootCJ = leftFoot.GetComponent<ConfigurableJoint>();
        rightFootCJ = rightFoot.GetComponent<ConfigurableJoint>();
        leftHandCJ = leftFoot.GetComponent<ConfigurableJoint>();
        rightHandCJ = rightFoot.GetComponent<ConfigurableJoint>();

        rb = body.GetComponent<Rigidbody>();
        leftFootRB = leftFoot.GetComponent<Rigidbody>();
        rightFootRB = rightFoot.GetComponent<Rigidbody>();
        leftHandRB = leftFoot.GetComponent<Rigidbody>();
        rightHandRB = rightFoot.GetComponent<Rigidbody>();

        leftFootCollider = leftFoot.GetComponent<Collider>();
        rightFootCollider = rightFoot.GetComponent<Collider>();
        leftHandCollider = leftHand.GetComponent<Collider>();
        rightHandCollider = rightHand.GetComponent<Collider>();
        bodyCollider = GetComponent<Collider>();
    }
}
