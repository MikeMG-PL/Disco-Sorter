using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuEditorPanel;  // Panel z opcjami odpalenia edytora, znajdujący się w menu głównym
    public GameObject allSavesPanel;        // Panel z przyciskami wszystkich zapisów

    private GameObject activePanel;         // Aktualnie aktywny panel
    private GameObject[] savesButtons;      // Tablica przycisków, które należą do panelu zapisów

    private void Start()
    {
        savesButtons = new GameObject[allSavesPanel.transform.childCount];
        for (int i = 0; i < allSavesPanel.transform.childCount; i++)
        {
            savesButtons[i] = allSavesPanel.transform.GetChild(i).gameObject;
        }
    }

    // Skrypty wykorzystywane w głównym menu

    // Opdala scenę z grą
    public void OpenGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    // Otwiera menu z wyborem (1) otwarcia pustego projektu lub (2) załadowania istniejącego projektu do edytora
    public void OpenEditorMenu()
    {
        ChangeActivePanel(mainMenuEditorPanel);
    }

    // Wczytuje dany projekt, zapisując dokonany wybór
    public void OpenEditorLoad(int selectedButton)
    {
        MenuSelectedOption.editorLoaded = true;
        MenuSelectedOption.selectedSong = selectedButton;
        SceneManager.LoadScene(1);
    }

    // Otwiera pusty projekt w edytorze
    public void OpenEditorNew()
    {
        SceneManager.LoadScene(1);
    }

    // Wyłącza aplikację
    public void Quit()
    {
        Application.Quit();
    }

    // Zmienia aktywne podmenu, lub zamyka aktualnie aktywne (jeśli nie podano nowego panelu do aktywacji)
    public void ChangeActivePanel(GameObject newPanel = null)
    {
        // Jeśli jakieś menu jest aktywne, wyłącza je
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        // Jeśli dano panel do aktywacji
        if (newPanel != null)
        {
            activePanel = newPanel;
            newPanel.SetActive(true);
        }
    }

    // Aktywuje (lub zamyka) panel z zapisami i aktualizuje napisy na przyciskach
    public void ShowSaves()
    {
        string[] songNames = GetSavesNames();

        if (allSavesPanel.activeSelf) ChangeActivePanel();
        else ChangeActivePanel(allSavesPanel);

        UpdateSavesNames(songNames);
    }


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


    // Zmienia tekst każdego przycisku, na odpowiadającą nazwę piosenki
    private void UpdateSavesNames(string[] songNames)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < songNames.Length)
                savesButtons[i].GetComponentInChildren<Text>().text = songNames[i];
            else
                savesButtons[i].GetComponentInChildren<Text>().text = "Puste";

            //Debug.Log(buttons[i].GetComponentInChildren<Text>().text);
        }
    }
}
