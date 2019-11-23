using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManipulation : MonoBehaviour
{
    public float time;
    AudioSource a;
    bool pausePressed = false;

    void Start()
    {
        a = GetComponent<AudioSource>();
        a.time = time;
        a.Play();
    }

    public void PausePlay()
    {
        if (pausePressed == false)
        {
            GetComponent<SongController>().enabled = false;
            time = a.time;
            pausePressed = !pausePressed;
            a.Stop();
        }
        else
        {
            GetComponent<SongController>().enabled = true;
            a.time = time;
            pausePressed = !pausePressed;
            a.Play();
        }


    }
}
