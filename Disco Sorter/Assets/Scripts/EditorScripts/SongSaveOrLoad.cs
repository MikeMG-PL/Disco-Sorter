using UnityEngine;

public class SongSaveOrLoad : MonoBehaviour
{
    public GameObject savesPanel;       // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania
    private EditorNet editorNet;

    private void Awake()
    {
        editorNet = GetComponent<EditorNet>();
    }

    public void SaveSong()
    {
        SongFile.SaveSong(gameObject.GetComponent<EditorNet>());
    }

    // Wczytuje dany plik, na podstawie int przekazanego przez przycisk znajdujący się w savesPanel
    public void LoadSong(int selectedButton)
    {
        savesPanel.SetActive(false);
        string[] songNames = SongFile.GetSavesNames();

        if (selectedButton >= songNames.Length)
            return;

        SongData songData = SongFile.LoadSong(songNames[selectedButton]);

        editorNet.BPM = songData.BPM;
        editorNet.netDensity = songData.netDensity;
        editorNet.BuildNet();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            editorNet.entityArray[i].GetComponent<Entity>().entityType = songData.entityType[i];
            editorNet.entityArray[i].GetComponent<Entity>().color = songData.color[i];
            editorNet.entityArray[i].GetComponent<Entity>().action = songData.action[i];
            editorNet.entityArray[i].GetComponent<Entity>().ChangeColor();
            editorNet.entityArray[i].GetComponent<Entity>().ChangeTypeIcon();
        }
    }
}
