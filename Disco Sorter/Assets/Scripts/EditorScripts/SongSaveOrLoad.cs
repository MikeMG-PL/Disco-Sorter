using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSaveOrLoad : MonoBehaviour
{
    private EditorNet editorNet;

    private void Start()
    {
        editorNet = GetComponent<EditorNet>();
    }

    public void SaveSong()
    {
        SongFile.SaveSong(gameObject.GetComponent<EditorNet>());
    }

    public void LoadSong(string songName)
    {
        SongData songData = SongFile.LoadSong(songName);

        editorNet.BPM = songData.BPM;
        editorNet.netDensity = songData.netDensity;
        Destroy(GameObject.FindGameObjectWithTag("Marker"));
        editorNet.BuildNet();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            editorNet.entityArray[i].GetComponent<Entity>().color = songData.color[i];
            editorNet.entityArray[i].GetComponent<Entity>().entityType = songData.entityType[i];
            editorNet.entityArray[i].GetComponent<Renderer>().material.color = editorNet.entityArray[i].GetComponent<Entity>().GetColor();
        }
    }
}
