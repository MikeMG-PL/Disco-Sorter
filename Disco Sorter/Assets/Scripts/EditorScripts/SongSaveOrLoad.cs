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

    Level level;

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
#if UNITY_EDITOR
        AssetDatabase.Refresh();
        string[] songNames = GetSavesNames();

        if (selectedButton >= songNames.Length || selectedButton < 0)
            return;


        string songPath = "Assets/SONGS/" + songNames[selectedButton] + ".mp3";
        AudioClip c = (AudioClip)AssetDatabase.LoadAssetAtPath(songPath, typeof(AudioClip));
        GetComponent<AudioSource>().clip = c;

        string levelPath = "Assets/LEVELS/" + songNames[selectedButton] + ".asset";
        Level level = (Level)AssetDatabase.LoadAssetAtPath(levelPath, typeof(Level));
#endif
        editorNet.BPM = level.BPM;
        editorNet.netDensity = level.netDensity;
        editorNet.songName = level.name;
        editorNet.BuildNet();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            entity.type = level.entityType[i];
            entity.color = level.color[i];
            entity.action = level.action[i];
            entity.linkedCatchEN = level.linkedCatchEN[i];
            entity.linkedReleaseEN = level.linkedCatchEN[i];
            entity.ChangeColor();
            entity.ChangeTypeIcon();
            entity.ChangeActionIcon();
        }


    }

    // Zwraca tablicę nazw zapisanych plików
    public static string[] GetSavesNames()
    {
        string[] savesNames = null;
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t: ScriptableObject", new[] { "Assets/LEVELS" });
        savesNames = guids;
        for (int i = 0; i < guids.Length; i++)
        {
            savesNames[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            savesNames[i] = savesNames[i].Remove(0, 14);
            savesNames[i] = savesNames[i].Remove(savesNames[i].IndexOf(".asset"), 6);
        }
#endif
        return savesNames;
    }


    private bool IsGoodToSave()
    {
        badEntities.Clear();

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();

            if (entity.type == EntityType.Apple && (entity.color == EntityColour.None || entity.action == EntityAction.None))
            {
                badEntities.Add(editorNet.entityArray[i]);
            }

            else if (entity.type == EntityType.RottenApple && entity.action == EntityAction.None)
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
