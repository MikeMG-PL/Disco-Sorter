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

    public void Restart()
    {
        a.time = 0f;
    }
    
    /// Funkcja odpowiedzialna za poprawne renderowanie i synchronizację waveformu ///
    public void Waveform()
    {
        MeshRenderer renderer = GameObject.FindGameObjectWithTag("Waveform").GetComponent<MeshRenderer>();
        DrawWaveForm waveForm = GetComponent<DrawWaveForm>();
        GameObject[] entityArray = GetComponent<EditorNet>().entityArray;
        EditorNet editorNet = GetComponent<EditorNet>();
        float sceneSongLength = editorNet.entitiesAmount * 0.1f + editorNet.entitiesAmount * 0.005f;
        Vector3 scaleVector = new Vector3(sceneSongLength, 1, 0.1f);

        // obliczenie długosci piosenki na scenie
        // utworzenie Vectora3 skali określającego porządaną długość 
        // przypisanie quadowi nowego vectora3 skali

        Debug.Log((int)sceneSongLength);
        renderer.gameObject.transform.localScale = scaleVector;

        //Mechanizm skalowania tekstury w zależności od długości piosenki i gęstości siatki
        if((int)sceneSongLength * 150 >= 60000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, 16000, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 48000 && (int)sceneSongLength * 150 < 60000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, (int)sceneSongLength * 40, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 36000 && (int)sceneSongLength * 150 < 48000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, (int)sceneSongLength * 50, 1000, Color.red); // liczba 150 - czułość wyświetlania waveformu
        else if ((int)sceneSongLength * 150 >= 24000 && (int)sceneSongLength * 150 < 36000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, (int)sceneSongLength * 65, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 16000 && (int)sceneSongLength * 150 < 24000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, (int)sceneSongLength * 100, 1000, Color.red);
        else if ((int)sceneSongLength * 150 < 16000)
            renderer.material.mainTexture = waveForm.PaintWaveformSpectrum(a.clip, 1f, (int)sceneSongLength * 150, 1000, Color.red);

        renderer.gameObject.transform.position = new Vector3(entityArray[0].transform.position.x-0.05f, entityArray[0].transform.position.y, entityArray[0].transform.position.z + 0.55f);
        renderer.material.mainTexture.filterMode = FilterMode.Point;
    }

}
