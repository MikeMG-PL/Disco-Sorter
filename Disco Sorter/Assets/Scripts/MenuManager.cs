using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject editorPanel;
    public GameObject savesPanel;
    private GameObject activePanel;

    public void OpenGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    // Otwiera menu z wyborem (1) otwarcia pustego projektu lub (2) załadowania istniejącego projektu do edytora
    public void OpenEditorMenu()
    {
        ChangeActivePanel(editorPanel);
    }

    // Wczytuje dany projekt
    public void OpenEditorLoad(int selectedButton)
    {
        MenuSelectedOption.editorLoad = true;
        MenuSelectedOption.selectedSong = selectedButton;
        SceneManager.LoadScene(1);
    }

    // Aktywuje (lub zamyka) panel z zapisami
    public void ShowSaves()
    {
        string[] songNames = SongFile.GetSavesNames();

        ChangeActivePanel(savesPanel);
        savesPanel.GetComponent<SavesManager>().UpdateSavesNames(songNames);
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

    // Zmienia aktywne podmenu
    private void ChangeActivePanel(GameObject newPanel)
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        if (newPanel != null)
        {
            activePanel = newPanel;
            newPanel.SetActive(true);
        }
    }
}
