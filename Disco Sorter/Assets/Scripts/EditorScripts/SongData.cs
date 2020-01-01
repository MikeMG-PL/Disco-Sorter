using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongData
{
    public List<int> entityNumber = new List<int>();
    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public int entitiesPerSecond;

    public SongData(EditorNet editorNet)
    {
        
        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            entityNumber.Add(editorNet.entityArray[i].GetComponent<Entity>().entityNumber);
            entityType.Add(editorNet.entityArray[i].GetComponent<Entity>().entityType);
            color.Add(editorNet.entityArray[i].GetComponent<Entity>().color);
        }

        entitiesPerSecond = editorNet.entitiesPerSecond;
    }
}
