using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class LoadToScene : MonoBehaviour
{
    private LevelParameters levelParameters;
    private LevelManager levelManager;

    [HideInInspector()]
    public Level level;

    /// ZAKTUALIZOWANIE ZAWARTOŚCI PRZYCISKÓW, WCZYTANIE PRZYCISKÓW ///
    private void Awake()
    {
        levelParameters = GetComponent<LevelParameters>();
        levelManager = GetComponent<LevelManager>();
        LoadSong();
    }

    /// PRZENIESIENIE DANYCH Z PLIKU DO SKRYPTU LEVELPARAMETERS ///
    public void LoadSong()
    {
        level = (Level)levelManager.buildLevels[levelManager.index];
        AudioClip c = levelManager.buildSongs[levelManager.index];
        GetComponent<AudioSource>().clip = c;

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
    }
}
