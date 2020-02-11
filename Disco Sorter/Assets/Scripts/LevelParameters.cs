using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParameters : MonoBehaviour
{
    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public List<int> action = new List<int>();
    public List<int> linkedReleaseEN = new List<int>();
    public List<int> linkedCatchEN = new List<int>();
    public int BPM, netDensity;
}