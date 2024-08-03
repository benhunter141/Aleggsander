using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }
    public ScriptableObjectHolder soHolder;
    public InputHandler inputHandler;
    public GizmoHolder gizmos;
    public Stats stats;
    public Animations animations;
    public UnitManager unitManager;
    public LayerManager layerManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        soHolder = GetComponentInChildren<ScriptableObjectHolder>();
    }
}
