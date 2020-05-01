using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SongSaveOrLoad : MonoBehaviour
{
    [SerializeField]
    private GameObject CameraHolder;

    [SerializeField]
    private GameObject savesPanel, entityMenu;       // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania. Obiekt zawierający skrypt entityMenu
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
        //SongFile.SaveSong(gameObject.GetComponent<EditorNet>());
        GetComponent<ScriptableObjectFactory>().CreateSO();
    }

    // Wczytuje dany plik, na podstawie int przekazanego przez przycisk znajdujący się w savesPanel
    public void LoadSong(int selectedButton)
    {



        savesPanel.SetActive(false);
        string[] songNames = GetSavesNames();

        if (selectedButton >= songNames.Length)
            return;

        AssetDatabase.Refresh();
        string[] assetGUID = AssetDatabase.FindAssets("t: ScriptableObject", new[] { "Assets/LEVELS/" + songNames[selectedButton] });
        string assetPath = assetGUID[0];
        assetPath = AssetDatabase.GUIDToAssetPath(assetPath);

        //SongData songData = SongFile.LoadSong(songNames[selectedButton]);

        /*editorNet.BPM = songData.BPM;
        editorNet.netDensity = songData.netDensity;
        editorNet.BuildNet();*/

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            // entity.type = songData.entityType[i];
            //entity.color = songData.color[i];
            // entity.action = songData.action[i];
            entity.ChangeColor();
            entity.ChangeTypeIcon();
            entity.ChangeActionIcon();
        }
    }

    // Zwraca tablicę nazw zapisanych plików
    public static string[] GetSavesNames()
    {
        string[] guids = AssetDatabase.FindAssets("t: ScriptableObject", new[] { "Assets/LEVELS" });
        string[] savesNames = guids;
        for (int i = 0; i < guids.Length; i++)
        {
            savesNames[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            savesNames[i] = savesNames[i].Remove(0, 14);
            savesNames[i] = savesNames[i].Remove(savesNames[i].IndexOf(".asset"), 6);
        }
        return savesNames;
    }

    private bool IsGoodToSave()
    {
        badEntities.Clear();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();

            if (entity.type == EntityType.Apple && (entity.color == 0 || entity.action == 0))
            {
                badEntities.Add(editorNet.entityArray[i]);
            }

            else if (entity.type == EntityType.RottenApple && entity.action == 0)
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
        Entity entity = badEntities[0].GetComponent<Entity>();

        // Wskazywanie kamerą na pierwszy źle ustawiony obiekt, nie na CameraHolder, ale na samą kamerę, wyłączenie podążania za markerem dla kamery
        float firstBadEntityPos = badEntities[0].transform.position.x;
        CameraHolder.GetComponent<EditorCamera>().MoveToPoint(firstBadEntityPos);

        // Otwieranie menu tego konkretnego obiektu
        entity.OpenThisEntityMenu();
    }
}
