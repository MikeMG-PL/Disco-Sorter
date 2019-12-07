using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManipulation : MonoBehaviour
{
    float time;
    float clampedLength;
    AudioSource a;
    bool pausePressed = false;
    public Slider slider;

    void Start()
    {
        a = GetComponent<AudioSource>();
        a.time = time;
        pausePressed = true;
    }

    public void Pause()
    {
        if (pausePressed == false)
        {
            GetComponent<SongController>().enabled = false;
            time = a.time;
            pausePressed = true;
            a.Stop();
        }
    }

    public void Play()
    {
        if (pausePressed == true)
        {
            GetComponent<SongController>().enabled = true;
            a.time = time;
            pausePressed = false;
            a.Play();
        }
    }

    void Clamp()
    {
        if (a.isPlaying)
        {
            clampedLength = a.time / a.clip.length;
            Debug.Log(clampedLength);
        }
    }

    void OnClipEnd()
    {
        if (!a.isPlaying && a.time > 0.1f)
            Pause();
    }

    public void Test()
    {
        Debug.Log("Klik!");
    }

    void Slider()
    {
        if (a.isPlaying)
        {
            slider.value = clampedLength;
        }
    }

    public void OnSliderRelease()
    {
        Debug.Log("Slider released!");
        time = slider.value * a.clip.length;
    }

    void Update()
    {
        Clamp();
        Slider();
        OnClipEnd();
    }
}
