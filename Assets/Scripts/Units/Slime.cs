using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : Unit
{


    private void OnCollisionEnter(Collision collision)
    {
        //if unit of differing allegiance, hurt them based on dmg * force
        if (!collision.gameObject.TryGetComponent<Unit>(out Unit hit)) return;
        if (hit.allegiance == allegiance) return; //no friendly fire, at least from slimes
        float magnitude = collision.impulse.magnitude;
        int roundedDamage = (int)(magnitude / stats.impulsePerDamage);
        hit.health.GetHitFor(roundedDamage);
    }



}
