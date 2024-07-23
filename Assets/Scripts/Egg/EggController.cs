using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public BodyParts bodyParts;
    public Brain brain;
    public Locomotion locomotion;
    public Tune tune;
    private void Awake()
    {
        brain = new Brain(this);
        locomotion = new Locomotion(this);
        tune = new Tune(this);
    }

    private void Start()
    {
        //bodyParts.rightHandCJ.anch
    }

    private void FixedUpdate()
    {
        locomotion.Process(brain);
        tune.Movement();
    }
}
