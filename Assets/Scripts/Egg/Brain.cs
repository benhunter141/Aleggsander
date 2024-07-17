using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    EggController egg;
    //these are set from input handler
    public Vector2 move, look;
    public Brain(EggController _egg)
    {
        egg = _egg;
    }
}
