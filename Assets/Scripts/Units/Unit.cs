using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public BodyParts bodyParts;
    public Brain brain;
    public PhysAnimator physAnimator;
    public Health health;
    public Movement move;
    public Targeting targeting;
    public Tune tune;
    public Leadership leadership;
    public Allegiance allegiance;
    public State currentState;
    public State aggroState, attackState, followState, deadState;


    //no formation
    //yes tune if you want tuner to update stuff like rigidbody or joint stats
    //no leadership
    //no equipment

    //new stats:
    public UnitStats stats;
    protected virtual void Awake()
    {
        brain = new Brain(this);
        move = new Movement(this);
        tune = new Tune(this);
        physAnimator = new PhysAnimator(this);
        health = new Health(this);
        targeting = new Targeting(this, stats.retargetFrequency);

        //state machine: specific to slime or egg so do it there
    }

    private void Start()
    {
        ServiceLocator.Instance.unitManager.allUnits.Add(this);
    }
}
