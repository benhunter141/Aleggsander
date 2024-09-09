using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscles
{
    Unit unit;

    public Muscles(Unit _unit)
    {
        unit = _unit;
    }

    //Methods here are used by PAs to move limbs each frame.
    public void HardPushLimbTo(ConfigurableJoint limb, Vector3 localPosition, float fMax, Vector3 limbCentre, float radius)
    {
        //adjust CA position, limit it to within reach
        Vector3 displacement = localPosition - limbCentre;
        if(displacement.magnitude > radius)
        {
            displacement.Normalize();
            displacement *= radius;
            localPosition = limbCentre + displacement;
        }
            
        limb.connectedAnchor = localPosition;
        //adjust spring value so that Fmax is constant
        Vector3 pullDisplacement = limb.connectedAnchor - limb.transform.localPosition;
        float pullDistance = pullDisplacement.magnitude;
        float spring = fMax / pullDistance;
        if (spring > unit.stats.maxSpring) spring = unit.stats.maxSpring;
        SoftJointLimitSpring limbSpring = limb.linearLimitSpring;
        limbSpring.spring = spring;
        limb.linearLimitSpring = limbSpring;
    }

    //void SoftPushLimbTo(ConfigurableJoint limb, Vector3 position, int frames, float fMax)
    //{
    //    throw new System.Exception("not setup");
    //    if(EnoughTime())
    //    {

    //    }
    //    else
    //    {
    //        HardPushLimbTo(limb, position, fMax);
    //    }

    //    bool EnoughTime()
    //    {
    //        //rb vel vs displacement over time
    //        Vector3 vel = unit.bodyParts.rb.velocity;
    //        Vector3 disp = unit.senses.Displacement();
    //        return true;
    //    }
    //}

}
