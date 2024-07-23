using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Formation : ScriptableObject
{
    public abstract List<Vector3> Positions(int count);
    public abstract List<Vector3> Directions(int count);
}
