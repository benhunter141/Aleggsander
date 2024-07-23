using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BattleLine")]
public class BattleLine : Formation
{
    public float spacing;
    public int rows;
    public override List<Vector3> Positions(int count)
    {
        var positions = new List<Vector3>();
        int unitsPerRow = count / rows;
        if (unitsPerRow * rows < count) unitsPerRow++;

        for(int i = 0; i < rows; i++)
        {
            float zCoord = -i * spacing;
            for(int j = 0; j < unitsPerRow; j++)
            {
                if (positions.Count == count) return positions;
                float xCoord;
                if(i % 2 == 0)
                {
                    xCoord = spacing * (j + 1);
                }
                else
                {
                    xCoord = -spacing * j;
                }
                Vector3 position = new Vector3(xCoord, 0, zCoord);
                positions.Add(position);
            }
        }
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
}
