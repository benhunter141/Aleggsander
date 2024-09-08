using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
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

    public PhysAction attack;

    private void OnCollisionEnter(Collision collision)
    {
        if (!BeingHeld()) return;
        if (!collision.gameObject.TryGetComponent<Unit>(out Unit _unit)) return;
        if (_unit == weilder) return;

        //Debug.Log($"v3 impulse: {collision.impulse.magnitude.ToString("F2")}");
        //Debug.Log($"Is weilder null?: {weilder is null}");
        //Debug.Log($"Is joint null?: {joint is null}");
        float magnitude = collision.impulse.magnitude;

        if (magnitude < minimumImpulseToDamage) return;

        _unit.health.GetHitFor((int)(collision.impulse.magnitude / minimumImpulseToDamage));
        //Debug.Log($"hit for dmg{(int)(collision.impulse.magnitude / minimumImpulseToDamage)}");

        Vector3 direction = -collision.contacts[0].normal;
        KnockBack(_unit, magnitude * direction);
    }

    void KnockBack(Unit _unit, Vector3 vector)
    {
        _unit.bodyParts.rb.AddForce(vector * knockBack, ForceMode.Impulse);
        //!!! Get the normal's vector???

        Debug.Log("applying knockback along vector: ");
        Helpers.PrintVector3(vector);
        Debug.DrawRay(_unit.transform.position, vector, Color.red, 1f);
    }
    public virtual void GetPickedUpBy(Unit egg) //snaps hand to weapon position & rotation, collider off, connects
    {
        Rigidbody hand = egg.bodyParts.rightHandRB;
        Collider handCollider = egg.bodyParts.rightHandCollider;

        handCollider.enabled = false;
        hand.transform.position = transform.position;
        hand.transform.rotation = transform.rotation;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand;
        weilder = egg;
        egg.weapon = this;
    }

    bool BeingHeld() => joint is not null;
}



