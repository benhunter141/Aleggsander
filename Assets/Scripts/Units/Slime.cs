using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : Unit
{
    State aggro, inMotion, sleep;

    private void OnCollisionEnter(Collision collision)
    {
        //if unit of differing allegiance, hurt them based on dmg * force
        if (!collision.gameObject.TryGetComponent<Unit>(out Unit hit)) return;
        if (hit.allegiance == allegiance) return; //no friendly fire, at least from slimes
        float magnitude = collision.impulse.magnitude;
        int roundedDamage = (int)(magnitude / stats.impulsePerDamage);
        hit.health.GetHitFor(roundedDamage);
    }
private void Start()
    {
        aggro = new Aggro(this);
        inMotion = new AttackState(this);
        sleep = new Sleep(this);
        
        //transitions
        
        aggro.transitions.Add(new Transition(this, aggro, inMotion, stats.atk.CanHit));
        inMotion.transitions.Add(new Transition(this, inMotion, aggro, physAnimator.IsIdling));
        aggro.transitions.Add(new Transition(this, aggro, sleep, targeting.SelfTargeting));
        //to do:
        //death + transitions to death
        //stun + transitions in and out of stun

        currentState = aggro;
    }

    private void FixedUpdate()
    {
        stats.Tune(this);
        currentState.Tick(); //state machine
        physAnimator.Process(); //actions (called by state machine)
    }
}
