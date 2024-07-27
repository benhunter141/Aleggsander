using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    EggController egg;
    public EggController leader;
    Vector2 cumulativeError;
    //these are set from input handler
    public Vector2 move, look;
    public Brain(EggController _egg)
    {
        egg = _egg;
    }

    public void ReceiveOrders(Order order) //every frame
    {
        //receiving WORLD pos and rots
        Vector3 displacementV3 = order.position - egg.transform.position;
        Vector2 displacement = Helpers.ConvertToV2(displacementV3);
        Vector2 direction = Helpers.ConvertToV2(order.direction);
        cumulativeError += displacement * ServiceLocator.Instance.soHolder.standardEggMoveStats.cumulativeMoveErrorFactor;
        cumulativeError *= ServiceLocator.Instance.soHolder.standardEggMoveStats.cumulativeMoveErrorDampener;
        displacement += cumulativeError;

        displacement *= ServiceLocator.Instance.soHolder.standardEggMoveStats.followerMoveMultiplier;
        if (displacement.sqrMagnitude > 1) displacement.Normalize();

        move = displacement;
        look = direction;
    }
}
