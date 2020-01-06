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
            editorNet.entityArray[i].GetComponent<Entity>().color = songData.color[i];
            editorNet.entityArray[i].GetComponent<Entity>().entityType = songData.entityType[i];
            editorNet.entityArray[i].GetComponent<Renderer>().material.color = editorNet.entityArray[i].GetComponent<Entity>().GetColor();
        }
    }

    // Aktywuje (lub zamyka) panel z zapisami i aktualizuje napisy na przyciskach
    public void ShowSaves()
    {
        string[] songNames = SongFile.GetSavesNames();

        savesPanel.SetActive(!savesPanel.activeSelf);
        savesPanel.GetComponent<SavesManager>().UpdateSavesNames(songNames);
    }
}
