using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionRing : MonoBehaviour
{
    //indicates allegiance with colour & direction with arrow
    public GameObject ring, arrow;
    public Unit egg;
    private void LateUpdate()
    {
        //point in flatfwd direction
        Vector3 direction = new Vector3(egg.brain.look.x, 0, egg.brain.look.y);
        direction = Helpers.FlatForward(direction);
        if(egg.brain.look == Vector2.zero)
        {
            transform.rotation = Quaternion.identity;
        }
        else transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        //ring and arrow height is always zero (change later if terrain)
        Vector3 position = egg.transform.position;
        position.y = 0f;
        transform.position = position;
    }

    public void ChangeRingColorTo(Material material)
    {
        ring.GetComponent<MeshRenderer>().material = material;
        arrow.GetComponent<MeshRenderer>().material = material;
    }
}
