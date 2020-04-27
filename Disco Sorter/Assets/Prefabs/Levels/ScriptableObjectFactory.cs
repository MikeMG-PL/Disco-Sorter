using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ScriptableObjectFactory : MonoBehaviour
{
    public Level levelTemplate; // BAZOWY SCRIPTABLE OBJECT POZIOMU
    Level currentLevel;
    new string name;

    void Start()
    {
        name = GetComponent<AudioSource>().clip.name;
    }

    public void CreateSO()
    {
        Level temp = (Level)UnityEngine.Object.Instantiate(levelTemplate);
        AssetDatabase.CreateAsset(temp, "Assets/Songs/" + name + ".asset");
        AssetDatabase.Refresh();
    }
}
