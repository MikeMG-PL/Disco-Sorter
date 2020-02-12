using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParameters : MonoBehaviour
{
    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public List<int> action = new List<int>();
    [Tooltip("Dla tego elementu listy, dla WYPUSZCZENIA, na tej kratce, połączone z nim ZŁAPANIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedCatchEN = new List<int>();
    [Tooltip("Dla tego elementu listy, dla tego ZŁAPANIA, na tej kratce, połączone z nim WYPUSZCZENIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedReleaseEN = new List<int>();
    public int BPM, netDensity;
}