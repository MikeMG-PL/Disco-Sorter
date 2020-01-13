using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SongFile
{
    private static string folderPath = Application.persistentDataPath + "/saves";
    private static string name = "song.data";

    // Zapisuje aktualny stan obiektów w siatce do pliku
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

    // Wczytuje dany plik (danych siatki)
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

    // Zwraca tablicę nazw zapisanych plików
    public static string[] GetSavesNames()
    {
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        DirectoryInfo[] info = dir.GetDirectories();

        string[] savesNames = new string[Directory.GetDirectories(folderPath).Length];
        for (int i = 0; i < savesNames.Length; i++)
        {
            savesNames[i] = info[i].Name;
        }

        return savesNames;
    }
}
