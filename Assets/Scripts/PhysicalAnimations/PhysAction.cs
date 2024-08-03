using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysAction : ScriptableObject
{
    public int totalFrames;
    public abstract void Do(Unit unit, int currentFrame);

    public virtual bool CanHit(Unit unit)
    {
        throw new System.Exception($"CanHit() not implemented for {name}");
    }
}
