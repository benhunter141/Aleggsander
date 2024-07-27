using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Formation : ScriptableObject
{
    
    public abstract List<Vector3> Positions(int count, EggController leader); //count is followers. Does not include leader.
    public abstract List<Vector3> Directions(int count);
    public Vector3 CentreOfFormation(EggController leader) => leader.transform.position;
    public void OrderList(EggController leader, ref List<EggController> followers)
    {
        //reorder followers so they are in order of positions they ought to receive
        var localPositions = Positions(followers.Count, leader);
        var centre = CentreOfFormation(leader);
        var orderedArray = new EggController[followers.Count];

        //pop followers off list, populate new one

        while (followers.Count != 0)
        {
            //find most extreme follower
            var furthestFollower = FurthestFollowerFrom(followers, centre);
            //find index of suitable position
            var positionIndex = PositionIndex(furthestFollower.transform.position);
            //place furthestFollower into array
            orderedArray[positionIndex] = furthestFollower;
            followers.Remove(furthestFollower);
        }

        followers = new List<EggController>(orderedArray);

        int PositionIndex(Vector3 eggPosition)
        {
            //what is the index of the position closest to eggPosition?
            Vector3 localPosition = leader.transform.InverseTransformPoint(eggPosition);
            //start with an index not yet taken
            int startingIndex = IndexNotYetTaken();
            for (int i = 0; i < localPositions.Count; i++)
            {
                if (orderedArray[i] is not null) continue;
                if (Vector3.SqrMagnitude(localPosition - localPositions[i]) >= Vector3.SqrMagnitude(localPosition - localPositions[startingIndex])) continue;
                startingIndex = i;
            }
            return startingIndex;
        }

        int IndexNotYetTaken()
        {
            for(int i = 0; i < orderedArray.Length; i++)
            {
                if (orderedArray[i] is null) return i;
            }
            throw new System.Exception("asdfasdfasdf");
        }

        EggController FurthestFollowerFrom(List<EggController> _followers, Vector3 centreWorldPos)
        {
            var furthest = _followers[0];
            for(int i = 1; i < _followers.Count; i++)
            {
                if (Vector3.SqrMagnitude(_followers[i].transform.position - centreWorldPos) <= Vector3.SqrMagnitude(furthest.transform.position - centreWorldPos)) continue;
                furthest = _followers[i];
            }
            return furthest;
        }
    }
    protected void UpdateGizmo(List<GameObject> gizmos, List<Vector3> localPositions, EggController leader)
    {
        //Debug.Log($"updating phalanx gizmo. gizmos count:{gizmos.Count}, pos count:{localPositions.Count}");
        while(gizmos.Count < localPositions.Count)
        {
            gizmos.Add(Instantiate(ServiceLocator.Instance.gizmos.gizmoPrefab, leader.transform));
        }
        //Debug.Log($"Instantiated...updating phalanx gizmo. gizmos count:{gizmos.Count}, pos count:{localPositions.Count}");
        for (int i = 0; i < gizmos.Count; i++)
        {
            gizmos[i].transform.localPosition = localPositions[i];
        }
    }
}
