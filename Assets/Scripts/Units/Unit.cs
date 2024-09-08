using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public BodyParts bodyParts;
    public BodyStats bodyStats;
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
    public PhysAction restPose;
    public Weapon weapon;
    public Senses senses;
    public Muscles muscles;

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
        senses = new Senses(this);
        muscles = new Muscles(this);
        behaviourTree.Initialize(sm);
        
        //state machine: specific to slime or egg so do it there
    }

    protected virtual void FixedUpdate()
    {
        //temp, delete:
        senses.LeanPID();

        //very old:: 
        stats.Tune(this);

        //state machine
        sm.currentState.Tick(); 
        sm.currentState.CheckForTransitions(sm);
        physAnimator.Process(); //actions (called by state machine)
                                //MOVE THIS TO STATE MACHINE?
                                //move.Process(brain); FOR EGGG

        
        //Balance, Stability
        senses.StabilityCheck();
        senses.PositionBalanceBaseGizmo();
    }

    private void Start()
    {
        ServiceLocator.Instance.unitManager.allUnits.Add(this);
    }

    public PhysAction BestAttack() => weapon is null ? stats.atk : weapon.attack;
    public bool CanHit(Unit _unit) => weapon is null ? stats.atk.CanHit(this) : weapon.attack.CanHit(this);
}
