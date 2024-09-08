using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysActionCollection")]
public class PhysActionCollection : ScriptableObject
{
    public List<PhysAction> actions;
    public PhysAction ActionWithName(string _name) => actions.Find(x => x.name == _name);
}
