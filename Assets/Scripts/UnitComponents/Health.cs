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
        currentHealth = unit.stats.totalHealth;
    }

    public void GetHitFor(int damage)
    {
        if (damage == 0) return;
        ServiceLocator.Instance.damagePopupManager.ShowDamage(unit, damage);
        currentHealth -= damage;
        ChangeEyeRedness();
        //Debug.Log($"hit for {damage}, health is now: {currentHealth}");
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
        //Debug.Log($"Turning off egg:", unit.gameObject);
        unit.gameObject.SetActive(false);
    }

    public void ChangeEyeRedness()
    {
        float healthPercent = (float)currentHealth / unit.stats.totalHealth;
        //
        //
        //float redness = 1 - healthPercent;
        Renderer left = unit.bodyParts.leftEyeRenderer;
        Renderer right = unit.bodyParts.rightEyeRenderer;
        Color color = new Color(1f, healthPercent, healthPercent);
        left.material.color = color;
        right.material.color = color;
    }
}
