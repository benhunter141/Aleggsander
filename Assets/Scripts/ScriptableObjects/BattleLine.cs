using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BattleLine")]
public class BattleLine : Formation
{
    public float spacing;
    public int rows;

    public override List<Vector3> Positions(int count, Unit leader)
    {
        //Debug.Log("battle line positions called");
        var positions = new List<Vector3>();
        int unitsPerRow = (count + 1) / rows;
        if (unitsPerRow * rows < count + 1) unitsPerRow++;

        for(int i = 0; i < rows; i++)
        {
            float zCoord = -i * spacing;
            for(int j = 0; j < unitsPerRow; j++)
            {
                if (i == 0 && j == 0) continue;
                if (positions.Count == count) break;
                float xCoord;
                if(j % 2 == 0)
                {
                    xCoord = -spacing * (j)/2;
                }
                else
                {
                    xCoord = spacing * (j+1)/2;
                }
                Vector3 position = new Vector3(xCoord, 0, zCoord);
                positions.Add(position);
            }
        }
        //Debug.Log("call update gizmos now. printing local positions");
        foreach(var p in positions)
        {
            
            //Helpers.PrintVector3(p);
        }
        UpdateGizmo(ServiceLocator.Instance.gizmos.gizmos, positions, leader);
        return positions;
    }

    public override List<Vector3> Directions(int count)
    {
        var directions = new List<Vector3>();
        for(int i = 0; i < count; i++)
        {
            directions.Add(Vector3.forward);
        }
        return directions;
    }

    //public override void OrderList(ref List<EggController> followers)
    //{
    //    throw new System.NotImplementedException();
    //}
}
