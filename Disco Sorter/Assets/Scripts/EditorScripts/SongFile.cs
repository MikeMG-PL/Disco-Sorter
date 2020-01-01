using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SongFile
{
    private static string folderPath = Application.persistentDataPath;
    private static string name = "song.data";

    public static void SaveSong(EditorNet editorNet)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Directory.CreateDirectory(folderPath + "/" + editorNet.songName);
        string filePath = Path.Combine(folderPath, editorNet.songName, name);
        Debug.Log(filePath);
        FileStream stream = new FileStream(filePath, FileMode.Create);
        SongData songData = new SongData(editorNet);

        formatter.Serialize(stream, songData);
        stream.Close();
    }

    public static SongData LoadSong(string songName)
    {
        string filePath = Path.Combine(folderPath, songName, name);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Save file not found in" + filePath);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Open);

        SongData songData = formatter.Deserialize(stream) as SongData;
        stream.Close();

        return songData;
    }
}
