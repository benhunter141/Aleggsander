using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/EggShape")]
public class EggShapes : ScriptableObject
{
    //height, waistRadius, ratio, vertsPerCircle, circles
    public float height, waistRadius, ratio, vertsPerCircle, circles;
}
