using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    //Weapon
    //On Equip / Awake, change mass of hand to 1 + weapon mass
    public float weaponMass, handMass;
   
    Rigidbody rb;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        if (rb is null) Debug.Log("ERROR ! ! ! ");

        rb.mass = weaponMass + handMass;
    }
}
