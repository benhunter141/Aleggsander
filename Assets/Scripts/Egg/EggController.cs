using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public BodyParts bodyParts;
    public Formation formation;

    public Brain brain;
    public Movement locomotion;
    public Tune tune;
    //public Recruit recruit;
    public Leadership leadership;
    //public Follow follow;

   
    private void Awake()
    {
        brain = new Brain(this);
        locomotion = new Movement(this);
        tune = new Tune(this);
        leadership = new Leadership(this, formation);
    }

    private void Update()
    {
        if (leadership.followers.Count > 0) leadership.SendOutOrders();
        tune.Movement();
    }

    private void FixedUpdate()
    {
        locomotion.Process(brain);
    }
}
