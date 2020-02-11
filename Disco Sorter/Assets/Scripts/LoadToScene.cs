using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadToScene : MonoBehaviour
{
    [SerializeField]
    private GameObject savesPanel, entityMenu;       // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania. Obiekt zawierający skrypt entityMenu
    private EditorNet editorNet;

    private void Awake()
    {
        editorNet = GetComponent<EditorNet>();
    }

    public void SaveSong(bool forceSave)
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
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            entity.type = songData.entityType[i];
            entity.color = songData.color[i];
            entity.action = songData.action[i];
            entity.ChangeColor();
            entity.ChangeTypeIcon();
            entity.ChangeActionIcon();
        }
    }
}
