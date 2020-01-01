using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSaveOrLoad : MonoBehaviour
{
    public void SaveSong()
    {
        SongFile.SaveSong(gameObject.GetComponent<EditorNet>());
    }

    public void LoadSong(string songName)
    {
        SongData songData = SongFile.LoadSong(songName);
    }
}
