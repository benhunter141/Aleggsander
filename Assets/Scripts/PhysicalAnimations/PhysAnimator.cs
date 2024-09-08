using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        if (animation is null) throw new System.Exception("passed a null PA, fucked up name?");
        inProgress.Add(new AnimationInProgress(animation, 0));
    }

    public void ClearAllAnimations()
    {
        inProgress.Clear();
        if(unit.restPose is not null) StartAnimation(unit.restPose);
    }

    public void ClearOtherAnimations(PhysAction action)
    {
        inProgress = inProgress.Where(x => x.physAnimation == action).ToList();
        if (unit.restPose is not null)
        {
            Debug.Log("starting T pose");
            StartAnimation(unit.restPose);
        }
            Debug.Log("PAs cancelled");
    }

    public bool IsWalking() => inProgress.Exists(x => x.physAnimation is Walk);
    public bool IsChopping() => inProgress.Exists(x => x.physAnimation is Chop);
    public bool IsAnimating(PhysAction pa) => inProgress.Exists(x => x.physAnimation == pa);
    public int AnimationCount() => inProgress.Count;
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

            inProgress[i].currentFrame++;

            if (inProgress[i].currentFrame == inProgress[i].physAnimation.totalFrames)
            {
                inProgress.RemoveAt(i);
                i--;
                continue;
            }
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

