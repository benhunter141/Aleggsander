using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangler : MonoBehaviour
{
    public float height;
    public Unit unit;
    private void Awake()
    {
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        Vector3 pos = unit.transform.position;
        pos.y = height;
        transform.position = pos;
    }
}
