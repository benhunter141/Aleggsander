using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : State
{
    Unit unit;
    
    Vector3 target;

    public Aggro(Unit _unit)
    {
        unit = _unit;
        transitions = new List<Transition>();
    }

    public override void OnEnter()
    {
        Transition();
        //show angry emoticon
    }

    public override void Tick()
    {

        unit.targeting.RetargetTick();
        if (!Moving()) Move();
        if (!Turning()) Turn();
        Transition();
    }

    bool Moving() => unit.physAnimator.IsAnimating(unit.stats.move);
    bool Turning() => unit.physAnimator.IsAnimating(unit.stats.turn);


    void Move() => unit.physAnimator.StartAnimation(unit.stats.move);
    void Turn() => unit.physAnimator.StartAnimation(unit.stats.turn);

}
