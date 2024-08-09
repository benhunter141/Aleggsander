using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }
    public ScriptableObjectHolder soHolder; //deprecated
    public InputHandler inputHandler;
    public GizmoHolder gizmos;
    public Animations animations; //deprecated
    public UnitManager unitManager;
    public LayerManager layerManager;
    public DamagePopupManager damagePopupManager;

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
