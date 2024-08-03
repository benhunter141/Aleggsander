using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysAnimator
{
    Unit unit;
    List<AnimationInProgress> inProgress; //holds frame info in here and runs animations to completion
    public PhysAnimator(Unit _unit)
    {
        unit = _unit;
        inProgress = new List<AnimationInProgress>();
    }

    public void StartAnimation(PhysAction animation) //call... wherever?
    {
        inProgress.Add(new AnimationInProgress(animation, 0));
    }

    public bool IsWalking() => inProgress.Exists(x => x.physAnimation is Walk);
    public bool IsChopping() => inProgress.Exists(x => x.physAnimation is Chop);
    public bool IsAnimating(PhysAction pa) => inProgress.Exists(x => x.physAnimation == pa);
    public bool IsIdling(Unit _unit)
    {
        //Debug.Log($"idle check, PA running {inProgress.Count} animations");
        if (inProgress.Count == 0)
        {
            return true;
        }
        else return false;
    }
    public void Process() //i want queueing and simultaneous functionality depending on context...
    {
        for (int i = 0; i < inProgress.Count; i++)
        {
            inProgress[i].physAnimation.Do(unit, inProgress[i].currentFrame);
            //Debug.Log($"just did animation: {inProgress[i].physAnimation.name}, frame:{inProgress[i].currentFrame}");
            if(inProgress[i].currentFrame == inProgress[i].physAnimation.totalFrames)
            {
                inProgress.RemoveAt(i);
                i--;
                continue;
            }
            inProgress[i].currentFrame++;
        }
    }
}

class AnimationInProgress
{ 
    public PhysAction physAnimation;
    public int currentFrame;

    public AnimationInProgress(PhysAction pa, int cf)
    {
        physAnimation = pa;
        currentFrame = cf;
    }
}

