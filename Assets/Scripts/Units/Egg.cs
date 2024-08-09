using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Unit
{

    public Formation formation;
    public Equipment equipment;

   
    protected override void Awake()
    {
        base.Awake();
        //leadership = new Leadership(this, formation);
    }

    //private void Update()
    //{
    //    if (leadership.followers.Count > 0) leadership.SendOutOrders();
    //}
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //move.Process(brain); //deprecated, work UprightForce into a physAnimation used by states
    }
}
