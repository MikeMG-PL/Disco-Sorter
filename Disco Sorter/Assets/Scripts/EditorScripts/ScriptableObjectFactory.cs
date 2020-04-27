using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ScriptableObjectFactory : MonoBehaviour
{
    public Level levelTemplate; // BAZOWY SCRIPTABLE OBJECT POZIOMU
    Level temp; string pathName;

    void Start()
    {
        pathName = GetComponent<AudioSource>().clip.name;
    }

    public void CreateSO()
    {
        temp = Instantiate(levelTemplate);

        SetValues(temp, GetComponent<EditorNet>());

        AssetDatabase.CreateAsset(temp, "Assets/LEVELS/" + pathName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void SetValues(Level level, EditorNet editorNet)
    {
        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            level.entityType.Add(entity.type);
            level.color.Add(entity.color);
            level.action.Add(entity.action);
            level.linkedReleaseEN.Add(entity.linkedReleaseEN);
            level.linkedCatchEN.Add(entity.linkedCatchEN);

        }
        level.name = GetComponent<AudioSource>().clip.name;
        level.BPM = editorNet.BPM;
        level.netDensity = editorNet.netDensity;
        level.clipLength = editorNet.gameObject.GetComponent<AudioManipulation>().floatClipLength;
    }
}
