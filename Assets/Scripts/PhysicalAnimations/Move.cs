using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/Move")]
public class Move : PhysAction
{
    //1 frame animation
    public override void Do(Unit unit, int currentFrame)
    {
        //doesn't matter what frame it is
        Vector3 forceDirection = Helpers.ConvertToV3(unit.brain.move);
        unit.bodyParts.rb.AddForce(forceDirection * unit.stats.moveForce);
    }
}
