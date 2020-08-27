using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudioManipulation : MonoBehaviour
{
    [HideInInspector()]
    public AudioSource aSrc;

    void Awake()
    {
        aSrc = GetComponent<AudioSource>();
        aSrc.time = 0f;
    }

    private void Update()
    {
        MenuAfterFinish();
    }

    void MenuAfterFinish()
    {
        if (aSrc.time >= aSrc.clip.length)
            SceneManager.LoadScene("3.MENU");
    }
}
