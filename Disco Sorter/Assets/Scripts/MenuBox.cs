using System.Collections;
using System.Collections.Generic;
using OVR;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuSide { Main, Credits, Settings };

public class MenuBox : MonoBehaviour
{
    public MainMenuManager menu;
    bool redRunning, yellowRunning;
    public ChooseLevel chooseLevel;
    public MenuSide boxSide;
    public OnScreenMenu CreditScreen, SettingsScreen;
    public FadeScreen screen;

    private void Awake()
    {
        redRunning = false; yellowRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) return;

        if (other.transform.parent.CompareTag("Apple"))
        {
            if (boxSide == MenuSide.Main && other.transform.parent.GetComponent<ObjectParameters>().color == EntityColour.Green) Play();
            else if (boxSide == MenuSide.Credits && other.transform.parent.GetComponent<ObjectParameters>().color == EntityColour.Red) Credits();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<BoxCollider>().isTrigger == false)
            Destroy(collision.gameObject);
    }

    private void Play()
    {
        menu.blocked = true;
        //chooseLevel.SpawnDiscos();
        StartCoroutine(DISCOS());
    }

    private void Credits()
    {
        if (!redRunning)
            StartCoroutine(CREDITS());
    }

    private void Settings()
    {

    }

    public IEnumerator CREDITS()
    {
        redRunning = true;
        CreditScreen.StartCredits();
        yield return new WaitForSecondsRealtime(14);
        redRunning = false;
        StopCoroutine(CREDITS());
    }

    public IEnumerator DISCOS()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        chooseLevel.SpawnDiscos();
        StopCoroutine(DISCOS());
    }
}
