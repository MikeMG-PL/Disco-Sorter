using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadToScene : MonoBehaviour
{
    [SerializeField]
    private GameObject savesPanel;                  // Panel, w którym znajdują się przyciski do wyboru pliku, do wczytania.
    private LevelParameters levelParameters;
    private string[] songNames;
    [SerializeField]
    private GameObject[] savesButtons;              // Poszczególne przyciski odpowiadające slotom zapisu piosenek

    /// ZAKTUALIZOWANIE ZAWARTOŚCI PRZYCISKÓW, WCZYTANIE PRZYCISKÓW ///
    private void Awake()
    {
        levelParameters = GetComponent<LevelParameters>();

        savesButtons = new GameObject[savesPanel.transform.childCount];
        for (int i = 0; i < savesPanel.transform.childCount; i++)
        {
            savesButtons[i] = savesPanel.transform.GetChild(i).gameObject;
        }

        songNames = SongFile.GetSavesNames();
        UpdateSavesNames(songNames);
    }

    /// PRZENIESIENIE DANYCH Z PLIKU DO SKRYPTU LEVELPARAMETERS ///
    public void LoadSong(int selectedButton)
    {
        if (selectedButton >= songNames.Length)
            return;

        // (!) Mikoś plz opisz dokładniej co oznaczają poszczególne inty (!) :3

        SongData songData = SongFile.LoadSong(songNames[selectedButton]);

        levelParameters.BPM = songData.BPM;
        levelParameters.netDensity = songData.netDensity;
        levelParameters.clipLength = songData.clipLength;

        for (int i = 0; i < songData.entityType.Count; i++)
        {
            levelParameters.entityType.Add(songData.entityType[i]);
            levelParameters.color.Add(songData.color[i]);
            levelParameters.action.Add(songData.action[i]);
            levelParameters.linkedCatchEN.Add(songData.linkedCatchEN[i]);
            levelParameters.linkedReleaseEN.Add(songData.linkedReleaseEN[i]);
        }

        levelParameters.Calculations();
        levelParameters.ConvertToPipeline();

    }

    /// UAKTUALNIANIE PRZYCISKÓW ///
    private void UpdateSavesNames(string[] songNames)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < songNames.Length)
                savesButtons[i].GetComponentInChildren<Text>().text = songNames[i];
            else
                savesButtons[i].GetComponentInChildren<Text>().text = "Puste";
        }
    }
}
