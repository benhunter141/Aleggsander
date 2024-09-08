using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Unit selectedEgg;
    public PlayerInput input;
    public InputAction move, look;

    //on leftstick input change, tell selectedEgg to change it's intent
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        move = input.actions.FindAction("Move");
        look = input.actions.FindAction("Look");
        if (selectedEgg is null) Debug.Log("no selected egg to receive inputs");
    }

    public void Update()
    {
        //Debug.Log("Controls just changed");
        if (selectedEgg is null) return;
        selectedEgg.brain.move = move.ReadValue<Vector2>();
        Vector2 lookInput = look.ReadValue<Vector2>();
        if (lookInput != Vector2.zero) selectedEgg.brain.look = lookInput;
    }
}
