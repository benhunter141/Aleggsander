using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSphere : MonoBehaviour
{
    public Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Egg>(out Egg egg)) return;
        Debug.Log("picked up");
        weapon.GetPickedUpBy(egg);

        gameObject.SetActive(false);
    }
}
