using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leadership
{
    EggController egg;
    public List<EggController> followers;
    Formation formation;

    public Leadership(EggController _egg, Formation _formation)
    {
        egg = _egg;
        formation = _formation;
        followers = new List<EggController>();
    }

    public void SendOutOrders() //done every frame, send out world pos/rots
    {
        var positions = formation.Positions(followers.Count, egg);
        var directions = formation.Directions(followers.Count);
        for (int i = 0; i < positions.Count; i++)
        {
            var follower = followers[i];
            var worldPos = egg.transform.TransformPoint(positions[i]);
            var worldDir = egg.transform.TransformDirection(directions[i]);
            Order order = new Order(worldPos, worldDir);
            follower.brain.ReceiveOrders(order);
        }
    }

    public void Recruit(EggController follower)
    {
        if (IsLeader())
        {
            followers.Add(follower);
            formation.OrderList(egg, ref followers);
            follower.brain.leader = egg;
        }
        else if (IsFollower())
        {
            Debug.Log("recruited by follower", egg.gameObject);
            Debug.Log("leader:", egg.brain.leader.gameObject);
            egg.brain.leader.leadership.Recruit(follower);
        }
        else throw new System.Exception("neither leader nor follower");
    }

    public bool IsLeader() => ServiceLocator.Instance.inputHandler.selectedEgg == egg;
    public bool IsFollower() => egg.brain.leader is not null;
}

public class Order
{
    public Vector3 position;
    public Vector3 direction;

    public Order(Vector3 pos, Vector3 dir)
    {
        position = pos;
        direction = dir;
    }
}

