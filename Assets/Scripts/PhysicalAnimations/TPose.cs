using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/TPose")]
public class TPose : PhysAction //1 frame ready position used for transitions
{
    public LocalPosition rightHandRest;
    public LocalPosition leftHandRest;
    public override void Do(Unit egg, int currentFrame)
    {
        var rhcj = egg.bodyParts.rightHandCJ;
        var lhcj = egg.bodyParts.leftHandCJ;
        rhcj.connectedAnchor = rightHandRest.pos;
        lhcj.connectedAnchor = leftHandRest.pos;
    }
}
