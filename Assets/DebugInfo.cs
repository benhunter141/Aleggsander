using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInfo : MonoBehaviour
{
    //displays current state
    //frames in state
    //leaning info, fwd back r l
    public Unit unit;
    public TextMeshProUGUI tmp;

    private void LateUpdate()
    {
        string state = unit.sm.currentState.ToString();
        string frames = unit.sm.framesInState.ToString();

        Vector2 lean = unit.senses.LeanPID();
        string leanX = lean.x.ToString();
        string leanY = lean.y.ToString();
        Vector3 localLean = unit.senses.LocalLeanPID();
        string text = $"State:{state} \n" +
            $"Frames:{frames} \n" +
            $"Global Lean Y:{leanY} \n" +
            $"Global Lean X:{leanX} \n" +
            $"Local Lean Y:{localLean.z} \n" +
            $"Local Lean X:{localLean.x}";
        tmp.text = text;
    }
}
