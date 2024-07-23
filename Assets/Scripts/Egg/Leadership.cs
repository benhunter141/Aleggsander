using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leadership
{
    EggController egg;
    List<EggController> followers;
    public Leadership(EggController _egg)
    {
        egg = _egg;
    }

    public void AddFollower(EggController follower)
    {
        followers.Add(follower);
    }

    //Input Handler tells this script to pass out orders to followers
    public void IssueOrders()
    {

    }
}
