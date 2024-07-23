using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    EggController egg;
    EggController leader;
    //these are set from input handler
    public Vector2 move, look;
    public Brain(EggController _egg)
    {
        egg = _egg;
    }

    public void ReceiveOrders(Vector3 destination, Vector3 direction)
    {
        Vector3 displacement = destination - egg.transform.position;
        
        move = new Vector2(displacement.x, displacement.z);
        look = new Vector2(direction.x, direction.z);
    }
}
