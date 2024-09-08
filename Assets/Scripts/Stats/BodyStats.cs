using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/BodyStats")]
public class BodyStats : ScriptableObject //THIS IS THE FUTURE, Egg.movestats is deprecated
{
    //Rest positions
    public Vector3 RightHandRestPos() => new Vector3(handLateralDistance, handRestHeight);
    public Vector3 LeftHandRestPos() => new Vector3(-handLateralDistance, handRestHeight);
    public Vector3 RightFootRestPos() => new Vector3(footLateralDistance, footRestHeight);
    public Vector3 LeftFootRestPos() => new Vector3(-footLateralDistance, footRestHeight);

    //public float armLength, legLength;

    public float handRestHeight, footRestHeight;
    public float handLateralDistance, footLateralDistance;
    public float armLength, legLength;
    public float waistRadius, height;
    public float handForceMax, footForceMax;

    //back positions - rest pos is shoulder, sure it doesn't make sense but w/e
    public Vector3 RightHandForwardPos() => RightHandRestPos() + new Vector3(0, 0, armLength);
    public Vector3 RightHandBackPos() => RightHandRestPos() - new Vector3(0, 0, armLength);

    //overhead position
    public Vector3 RightHandOverheadPos() => new Vector3(waistRadius / 2, height, 0);
    public Vector3 LeftHandOverheadPos() => new Vector3(-waistRadius / 2, height, 0);
    //underfoot position
    public Vector3 RightUnderFootPos() => new Vector3(waistRadius / 2, 0, 0);
    public Vector3 LeftUnderFootPos() => new Vector3(-waistRadius / 2, 0, 0);


}
