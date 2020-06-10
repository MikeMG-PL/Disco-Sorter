using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public new string name;
    public string artist;
    public List<EntityType> entityType = new List<EntityType>();
    public List<EntityColour> color = new List<EntityColour>();
    public List<EntityAction> action = new List<EntityAction>();
    public List<int> linkedReleaseEN = new List<int>();
    public List<int> linkedCatchEN = new List<int>();
    public List<float> linkedReleaseTimeStart = new List<float>();
    public List<float> linkedReleaseTimeEnd = new List<float>();
    public List<float> linkedCatchTime = new List<float>();
    public int BPM;
    public int netDensity;
    public float clipLength;
}
