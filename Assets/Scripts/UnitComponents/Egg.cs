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
        currentState = new Follow(this);
        leadership = new Leadership(this, formation);
    }





    private void Update()
    {
        if (leadership.followers.Count > 0) leadership.SendOutOrders();
        tune.Movement(); //movement should be a physAction
    }

    private void FixedUpdate()
    {
        //MOVE THIS TO STATE MACHINE
        move.Process(brain);

        currentState.Tick(); //state machine
        physAnimator.Process(); //actions (called by state machine)
    }
}
