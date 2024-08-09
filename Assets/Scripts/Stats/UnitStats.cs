using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/UnitStats")]
public class UnitStats : ScriptableObject
{
    //Slime needs...
    public float impulsePerDamage;
    public float moveForce, mass, drag;
    public float dynamicFriction, staticFriction;
    public float originalScale;
    public int retargetFrequency;
    public int totalHealth;

    public PhysAction move, turn, atk;
    public State sleep;

    public void Tune(Unit unit)
    {
        unit.bodyParts.rb.mass = mass;
        unit.bodyParts.rb.drag = drag;
        var mat = unit.bodyParts.bodyCollider.material;
        mat.dynamicFriction = dynamicFriction;
        mat.staticFriction = staticFriction;
        unit.bodyParts.bodyCollider.material = mat;
    }
}
