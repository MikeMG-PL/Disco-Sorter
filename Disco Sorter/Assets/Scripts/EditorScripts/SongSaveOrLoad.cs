using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSaveOrLoad : MonoBehaviour
{
    [SerializeField]
    private GameObject CameraHolder;

    [SerializeField]
    private GameObject savesPanel, entityMenu, toggle;       // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania. Obiekt zawierający skrypt entityMenu
    private EditorNet editorNet;
    private List<GameObject> badEntities = new List<GameObject>();      // Lista źle ustawionych obiektów

    private void Awake()
    {
        editorNet = GetComponent<EditorNet>();
    }

    public void SaveSong(bool forceSave)
    {
        if (!forceSave && !IsGoodToSave()) return;
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

    private bool IsGoodToSave()
    {
        badEntities.Clear();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();

            if (entity.entityType == 1 && (entity.color == 0 || entity.action == 0))
            {
                badEntities.Add(editorNet.entityArray[i]);
            }

            else if (entity.entityType == 2 && entity.action == 0)
            {
                badEntities.Add(editorNet.entityArray[i]);
            }
        }

        if (badEntities.Count != 0)
        {
            PointBadEntity();
            return false;
        }

        return true;
    }

    // Wskazuje pierwszy źle ustawiony obiekt i otwiera jego menu
    private void PointBadEntity()
    {
        toggle.GetComponent<Toggle>().isOn = false;     // można spróbować te 2 linijki napisać bardziej elegancko, jest tu obecnie podejście YOLO przy robieniu UI
        CameraHolder.GetComponent<EditorCamera>().moveCamera = true;

        Entity entity = badEntities[0].GetComponent<Entity>();

        // Wskazywanie kamerą na pierwszy źle ustawiony obiekt, nie na CameraHolder, ale na samą kamerę, wyłączenie podążania za markerem dla kamery

        float firstBadEntityPos = badEntities[0].transform.position.x;
        CameraHolder.transform.GetChild(0).GetComponent<EditorCamera>().MoveToPoint(firstBadEntityPos);

        // Otwieranie menu tego konkretnego obiektu
        entity.OpenThisEntityMenu();
    }
}
