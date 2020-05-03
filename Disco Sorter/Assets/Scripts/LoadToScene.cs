using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class LoadToScene : MonoBehaviour
{
    [SerializeField]
    private GameObject savesPanel;                  // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania.
    private LevelParameters levelParameters;
    private LevelManager levelManager;
    private string[] songNames;
    [SerializeField]
    private GameObject[] savesButtons;              // Poszczególne przyciski odpowiadające slotom zapisu piosenek

    /// ZAKTUALIZOWANIE ZAWARTOŚCI PRZYCISKÓW, WCZYTANIE PRZYCISKÓW ///
    private void Awake()
    {
        levelParameters = GetComponent<LevelParameters>();
        levelManager = GetComponent<LevelManager>();

        savesButtons = new GameObject[savesPanel.transform.childCount];
        for (int i = 0; i < savesPanel.transform.childCount; i++)
        {
            savesButtons[i] = savesPanel.transform.GetChild(i).gameObject;
        }

        songNames = GetSavesNames();
        UpdateSavesNames(songNames);

        for (int i = 0; i < savesPanel.transform.childCount; i++)
        {
            levelManager.LevelList.Add(savesPanel.transform.GetChild(i).GetComponentInChildren<Text>().text);
        }

    }

    /// PRZENIESIENIE DANYCH Z PLIKU DO SKRYPTU LEVELPARAMETERS ///
    public void LoadSong(int selectedButton)
    {
        if (selectedButton >= songNames.Length || selectedButton < 0)
            return;

        // (!) Mikoś plz opisz dokładniej co oznaczają poszczególne inty (!) :3

        string levelPath = "Assets/LEVELS/" + songNames[selectedButton] + ".asset";
        Level level = (Level)AssetDatabase.LoadAssetAtPath(levelPath, typeof(Level));

        levelParameters.name = level.name;
        levelParameters.BPM = level.BPM;
        levelParameters.netDensity = level.netDensity;
        levelParameters.clipLength = level.clipLength;

        for (int i = 0; i < level.entityType.Count; i++)
        {
            levelParameters.entityType.Add(level.entityType[i]);
            levelParameters.color.Add(level.color[i]);
            levelParameters.action.Add(level.action[i]);
            levelParameters.linkedCatchEN.Add(level.linkedCatchEN[i]);
            levelParameters.linkedReleaseEN.Add(level.linkedReleaseEN[i]);
        }

        levelParameters.Calculations();
        levelParameters.ConvertToPipeline();

        string songPath = "Assets/SONGS/" + songNames[selectedButton] + ".mp3";
        AudioClip c = (AudioClip)AssetDatabase.LoadAssetAtPath(songPath, typeof(AudioClip));
        GetComponent<AudioSource>().clip = c;
    }

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

    /// UAKTUALNIANIE PRZYCISKÓW ///
    private void UpdateSavesNames(string[] songNames)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < songNames.Length)
                savesButtons[i].GetComponentInChildren<Text>().text = songNames[i];
            else
                savesButtons[i].GetComponentInChildren<Text>().text = "[PUSTE]";
        }
    }
}
