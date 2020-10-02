using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class FadeScreen : MonoBehaviour
{
    public VRTK_SDKManager SDKManager;

    private VRTK_SDKSetup SDKSetup;
    private bool wasFaded, loadingLevel;
    float timer;

    void Awake()
    {
        OnlyUnfade(1f);
        timer = 0;
    }

    // Szybkie zmienienie ekranu na czarny podczas odpalenia gry i późniejszy fade out
    // Musimy to robić w ten sposób ponieważ SDK potrzebne do fade'u nie jest ładowane na Awake, tylko jakiś czas **po** Start
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            OnlyFade(0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnlyUnfade(0.5f);
        }

        if (!wasFaded)
        {
            SDKSetup = SDKManager.loadedSetup;
            if (SDKSetup != null)
            {
                OnlyFade(0);
                wasFaded = true;
                OnlyUnfade(1f);
            }
        }

        if(loadingLevel)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= 1f)
                SceneManager.LoadScene("2.GAME");
        }
    }

    // Funkcja używana podczas odpalania gry z menu
    public void FadeInAndStartGame()
    {
        GetComponent<VRTK_HeadsetFade>().Fade(Color.black, 0.5f);
        loadingLevel = true;
    }

    public void OnlyFade(float t)
    {
        GetComponent<VRTK_HeadsetFade>().Fade(Color.black, t);
    }

    public void OnlyUnfade(float t)
    {
        GetComponent<VRTK_HeadsetFade>().Unfade(t);
    }
}