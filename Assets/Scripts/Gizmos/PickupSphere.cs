using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSphere : MonoBehaviour
{
    Weapon weapon;
    private void Awake()
    {
        weapon = transform.parent.GetComponent<Weapon>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Egg>(out Egg egg)) return; //only eggs can weild
        if (egg.weapon is not null) return;
        //Debug.Log("picked up");
        weapon.GetPickedUpBy(egg);

        gameObject.SetActive(false);
    }
}
