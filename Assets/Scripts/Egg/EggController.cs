using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public Rigidbody rb;
    //brain - sets goals like move & look
    //Locomotion - moves feet and also addforces body upright and fwd
    //Hands / Weapons - moves hands and weapons
    //Goals / Thoughts / Brain - Receives orders
    public Brain brain;
    public Locomotion locomotion;

    private void Awake()
    {
        brain = new Brain(this);
        locomotion = new Locomotion(this);
    }

    private void FixedUpdate()
    {
        locomotion.Process(brain);
    }
}
