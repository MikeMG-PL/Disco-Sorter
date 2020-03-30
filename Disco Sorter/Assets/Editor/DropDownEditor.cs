using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class DropDownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelManager script = (LevelManager)target;

        GUIContent levelList = new GUIContent("Level");
        script.levelIndex = EditorGUILayout.Popup(levelList, script.levelIndex, script.LevelList.ToArray());
    }
}
