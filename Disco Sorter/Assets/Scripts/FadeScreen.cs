using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class FadeScreen : MonoBehaviour
{
    public VRTK_SDKManager SDKManager;

    private VRTK_SDKSetup SDKSetup;
    private bool thisIsGame, wasFaded;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "2.GAME") thisIsGame = true;
    }

    // Szybkie zmienienie ekranu na czarny podczas odpalenia gry i późniejszy fade out
    // Musimy to robić w ten sposób ponieważ SDK potrzebne do fade'u nie jest ładowane na Awake, tylko jakiś czas **po** Start
    void Update()
    {
        if (!wasFaded && thisIsGame)
        {
            SDKSetup = SDKManager.loadedSetup;
            if (SDKSetup != null)
            {
                GetComponent<VRTK_HeadsetFade>().Fade(Color.black, 0f);
                wasFaded = true;
                GetComponent<VRTK_HeadsetFade>().Unfade(0.75f);
            }
        }
    }

    // Funkcja używana podczas odpalania gry z menu
    public IEnumerator FadeInAndStartGame()
    {
        GetComponent<VRTK_HeadsetFade>().Fade(Color.black, 0.5f);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("2.GAME");
    }
}