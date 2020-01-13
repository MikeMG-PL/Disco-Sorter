using UnityEngine;

public class SongSaveOrLoad : MonoBehaviour
{

    [SerializeField]
    private GameObject savesPanel, entityMenu;       // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania. Obiekt zawierający skrypt entityMenu

    private EditorNet editorNet;

    private void Awake()
    {
        editorNet = GetComponent<EditorNet>();
    }

    public void SaveSong()
    {
        entityMenu.GetComponent<EntityMenu>().IsSavedChange(true);
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
