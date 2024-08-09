using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Vector3 rightHandRestPosition, leftHandRestPosition;
    public Rigidbody rb;
    public Collider pickupTrigger;
    public Collider weaponHitBox;
    public Unit weilder;
    public FixedJoint joint;

    public float knockBack;
    public float minimumImpulseToDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (!BeingHeld()) return;
        if (!collision.gameObject.TryGetComponent<Unit>(out Unit egg)) return;
        if (egg == weilder) return;

        //Debug.Log($"v3 impulse: {collision.impulse.magnitude.ToString("F2")}");
        //Debug.Log($"Is weilder null?: {weilder is null}");
        //Debug.Log($"Is joint null?: {joint is null}");

        if (collision.impulse.magnitude < minimumImpulseToDamage) return;

        egg.health.GetHitFor((int)(collision.impulse.magnitude / minimumImpulseToDamage));
        //Debug.Log($"hit for dmg{(int)(collision.impulse.magnitude / minimumImpulseToDamage)}");
        KnockBack(egg, -collision.impulse);
    }

    void KnockBack(Unit _unit, Vector3 vector)
    {
        _unit.bodyParts.rb.AddForce(vector * knockBack, ForceMode.Impulse);
    }
    public void GetPickedUpBy(Unit egg) //snaps hand to weapon position & rotation, collider off, connects
    {
        Rigidbody hand = egg.bodyParts.rightHandRB;
        Collider handCollider = egg.bodyParts.rightHandCollider;

        handCollider.enabled = false;
        hand.transform.position = transform.position;
        hand.transform.rotation = transform.rotation;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand;
        weilder = egg;
    }

    bool BeingHeld() => joint is not null;
}



