using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    Unit unit;
    public Unit leader;
    Vector2 cumulativePositionError;
    //these are set from input handler
    public Vector2 move, look;
    public Brain(Unit _unit)
    {
        unit = _unit;
        
    }

    public void ReceiveTarget(Vector3 targetPosition)
    {
        Vector3 displacement = targetPosition - unit.transform.position;
        ReceiveOrders(new Order(targetPosition, displacement));
    }
    public void ReceiveOrders(Order order) //every frame if following, receive position and rotation info from leader
    {
        //receiving WORLD pos and rots
        Vector3 displacementV3 = order.position - unit.transform.position;
        Vector2 displacement = Helpers.ConvertToV2(displacementV3);
        Vector2 direction = Helpers.ConvertToV2(order.direction);
        //Integral allowing egg to cover last little bit of ground to get to destination::
        cumulativePositionError += displacement * ServiceLocator.Instance.soHolder.standardEggMoveStats.cumulativeMoveErrorFactor;
        cumulativePositionError *= ServiceLocator.Instance.soHolder.standardEggMoveStats.cumulativeMoveErrorDampener;
        displacement += cumulativePositionError;

        displacement *= ServiceLocator.Instance.soHolder.standardEggMoveStats.followerMoveMultiplier;
        if (displacement.sqrMagnitude > 1) displacement.Normalize();

        move = displacement;
        look = direction;
    }
}
