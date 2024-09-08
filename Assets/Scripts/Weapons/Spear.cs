using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    public float restRot;
    public override void GetPickedUpBy(Unit egg) //snaps hand to weapon position & rotation, collider off, connects
    {
        Rigidbody hand = egg.bodyParts.rightHandRB;
        Collider handCollider = egg.bodyParts.rightHandCollider;

        handCollider.enabled = false;
        hand.transform.position = transform.position;
        hand.transform.rotation = transform.rotation;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand;
        Debug.Log("set rot of right hand");
        ConfigurableJointExtensions.SetTargetRotationLocal(egg.bodyParts.rightHandCJ, 
            Quaternion.Euler(new Vector3(-restRot, 0, 0)), 
            egg.bodyParts.rightHandRot);
        weilder = egg;
        egg.weapon = this;
    }
}
