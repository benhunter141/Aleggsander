using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public Unit unit;
    public int currentHealth;

    public Health(Unit _egg)
    {
        unit = _egg;
        currentHealth = ServiceLocator.Instance.stats.body.health;
    }

    public void GetHitFor(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"hit for {damage}, health is now: {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        //turn everything off
        //destroy?
        //remove from leader's follower list
        if(unit.brain.leader is not null) unit.brain.leader.leadership.followers.Remove(unit);
        ServiceLocator.Instance.unitManager.allUnits.Remove(unit);
        TurnOffEverything();
    }

    public void TurnOffEverything()
    {
        Debug.Log($"Turning off egg:", unit.gameObject);
        unit.gameObject.SetActive(false);
    }
}
