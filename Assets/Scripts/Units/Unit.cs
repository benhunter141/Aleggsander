using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public BodyParts bodyParts;
    public Brain brain; //defines goals. eg. kill target. hold pos. rout. encircle.
    public PhysAnimator physAnimator;
    public Health health;
    public Movement move;
    public Targeting targeting; //finds units or other objects to target.
    public Tune tune;
    public Leadership leadership;
    public Allegiance allegiance;
    public StateMachine sm;
    public BehaviourTree behaviourTree;


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
        sm = new StateMachine(this);
        behaviourTree.Initialize(sm);

        //state machine: specific to slime or egg so do it there
    }

    protected virtual void FixedUpdate()
    {
        stats.Tune(this);
        sm.currentState.Tick(); //state machine
        physAnimator.Process(); //actions (called by state machine)
                                //MOVE THIS TO STATE MACHINE
                                //move.Process(brain); FOR EGGG
    }

    private void Start()
    {
        ServiceLocator.Instance.unitManager.allUnits.Add(this);
    }
}
