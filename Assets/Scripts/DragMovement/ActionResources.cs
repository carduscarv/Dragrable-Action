using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActionResources", menuName = "Test Drag 2D/ActionResources", order = 0)]
public class ActionResources : ScriptableObject {
    public new string name;
    public string type;
    public string action;
    public GameObject actionObject;
}
