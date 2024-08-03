using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggParts : BodyParts
{

    protected override void Awake()
    {
        base.Awake();

        leftFootCJ = leftFoot.GetComponent<ConfigurableJoint>();
        rightFootCJ = rightFoot.GetComponent<ConfigurableJoint>();
        leftHandCJ = leftHand.GetComponent<ConfigurableJoint>();
        rightHandCJ = rightHand.GetComponent<ConfigurableJoint>();

        leftFootRB = leftFoot.GetComponent<Rigidbody>();
        rightFootRB = rightFoot.GetComponent<Rigidbody>();
        leftHandRB = leftHand.GetComponent<Rigidbody>();
        rightHandRB = rightHand.GetComponent<Rigidbody>();

        leftFootCollider = leftFoot.GetComponent<Collider>();
        rightFootCollider = rightFoot.GetComponent<Collider>();
        leftHandCollider = leftHand.GetComponent<Collider>();
        rightHandCollider = rightHand.GetComponent<Collider>();

        leftFootRot = leftFoot.transform.localRotation;
        rightFootRot = rightFoot.transform.localRotation;
        leftHandRot = leftHand.transform.localRotation;
        rightHandRot = rightHand.transform.localRotation;
    }
}
