using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManipulation : MonoBehaviour
{
    public float time = 0f;                 // Zmienna opisująca porządany moment w piosence
    float clampedLength;                    // Zmienna opisująca porządany moment w piosence w przedziale <0; 1>
    AudioSource a;                          // Zmienna reprezentująca źródło dźwięku

    [HideInInspector]
    public bool pausePressed = false;       // Zmienna mówiąca czy został wciśnięty przycisk pauzy

    bool virtualPause;                      // Zmienna mówiąca czy jest włączona wirtualna pauza*
    public Slider slider;                   // Zmienna opisująca slider
    //public float latency = 0f;                  // Opóźnienie, z jakim odtwarzana jest piosenka

    // * - wirtualna pauza - pauza piosenki mogąca pojawić się bez wciśnięcia przycisku pauzy (bo wymaga tego edytor do niektórych celów)

    /// Pobranie źródła dźwięku, przewinięcie źródła dźwięku do początku, wciśnięcie pauzy oraz pauzy wirtualnej ///
    void Start()
    {
        a = GetComponent<AudioSource>();
        a.time = time;
        pausePressed = true;
        virtualPause = true;
    }

    /// Funkcja wykonująca co klatkę najważniejsze operacje ///
    void Update()
    {
        Clamp();
        Slider();
        OnClipEnd();
    }

    /// Funkcja opisująca odtwarzanie linii poprzez wciśnięcie przycisku ///
    public void Play()
    {
        if (pausePressed == true)
        {
            a.time = time;
            pausePressed = false;
            virtualPause = false;
            if (time < a.clip.length)
                a.Play();
            //if (time < 0.1f)
                //a.time += latency;
        }
    }

    /// Funkcja opisująca pauzowanie linii poprzez wciśnięcie przycisku ///
    public void Pause()
    {
        if (pausePressed == false)
        {
            time = a.time;
            pausePressed = true;
            virtualPause = true;
            a.Stop();
        }
    }

    /// Funkcja umożliwiająca przewijanie linii podczas gdy gra jest zapauzowana ///
    public void VirtualPlay()
    {
        if (pausePressed == true)
        {
            a.time = time;
            pausePressed = false;
            if (time < a.clip.length)
                a.Play();
        }
    }

    /// Funkcja "ściskająca długość piosenki w przedziale <0; 1> ///
    void Clamp()
    {
        if (a.isPlaying)
            clampedLength = a.time / a.clip.length;
    }

    /// Funkcja zatrzymująca linię po zakończeniu odtwarzania ///
    void OnClipEnd()
    {
        if ((!a.isPlaying && a.time > 0.1f) || a.time >= a.clip.length)
        {
            Pause();
            Play();
            Pause();
        }
    }

    /// Funkcja przenosząca "ściśniętą" długość (liczby) na slider ///
    void Slider()
    {
        if (a.isPlaying)
        {
            slider.value = clampedLength;
        }
    }

    /// Funkcja odpowiedzialna za przewijanie sliderem do przodu i do tyłu ///
    public void OnSliderMove()
    {
        if (!virtualPause)
        {
            time = slider.value * a.clip.length;
            if (time < a.clip.length)
                a.time = time;
        }
        else
        {
            Play();
            time = slider.value * a.clip.length;
            if (time < a.clip.length)
                a.time = time;
            virtualPause = true;
        }

    }

    /// Funkcja odpowiedzialna za pauzowanie linii, gdy puścimy slider, a przewijamy podczas zapauzowanej piosenki ///
    public void OnSliderRelease()
    {
        if (virtualPause)
            Pause();
    }

    
}
