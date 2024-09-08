using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellyToQuad : State
{
    Unit unit;
    PhysAction placeHands;
    public BellyToQuad(Unit _unit, PhysAction _placeHands)
    {
        unit = _unit;
        placeHands = _placeHands;
    }
    public override void OnEnter()
    {
        Debug.Log($"Belly to Quad");
    }

    public override void Tick()
    {
        unit.physAnimator.StartAnimation(placeHands);

    }
}
