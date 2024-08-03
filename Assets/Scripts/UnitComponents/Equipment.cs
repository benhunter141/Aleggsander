using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    Unit egg;
    Weapon right, left;
    public Equipment(Unit _egg)
    {
        egg = _egg;
    }

    public void PickupRightHand(Weapon weapon)
    {
        //walk towards weapon
        //move handCA towards weapon
        //snap on and attach
    }
}
