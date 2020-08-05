using OVR;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuSide { Main, Credits, Settings};

public class MenuBox : MonoBehaviour
{
    public ChooseLevel chooseLevel;
    public MenuSide boxSide;
    public OnScreenMenu CreditScreen, SettingsScreen;
    public FadeScreen screen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) return;

        if (other.transform.parent.CompareTag("Apple"))
        {
            if (boxSide == MenuSide.Main) Play();
            else if (boxSide == MenuSide.Credits) Credits();
            else if (boxSide == MenuSide.Settings) Settings();
        }
    }

    private void Play()
    {
        chooseLevel.SpawnDiscos();
    }

    private void Credits()
    {
        CreditScreen.StartCredits();
    }

    private void Settings()
    {

    }
}
