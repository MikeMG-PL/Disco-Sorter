using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public new string name;
    public List<EntityType> entityType = new List<EntityType>();
    public List<EntityColour> color = new List<EntityColour>();
    public List<EntityAction> action = new List<EntityAction>();
    public List<int> linkedReleaseEN = new List<int>();
    public List<int> linkedCatchEN = new List<int>();
    public int BPM;
    public int netDensity;
    public float clipLength;
}
