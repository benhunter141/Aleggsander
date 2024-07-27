using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitSphere : MonoBehaviour
{
    public EggController egg;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<EggController>(out EggController touchedEgg)) return;
        //Debug.Log("touched by egg");
        if (touchedEgg.leadership.IsLeader())
        {
            //Debug.Log("RECRUITED by leader!!!");
            if (egg is null) throw new System.Exception("egg is null... forgot to link?");
            touchedEgg.leadership.Recruit(egg);
            TurnOffSphere();
        }
        else if(touchedEgg.leadership.IsFollower())
        {
            //Debug.Log("RECRUITED!!! by follower");
            if (egg is null) throw new System.Exception("egg is null... forgot to link?");
            touchedEgg.brain.leader.leadership.Recruit(egg);
            TurnOffSphere();
        }
        
    }

    void TurnOffSphere()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<SphereCollider>().enabled = false;
    }
}
