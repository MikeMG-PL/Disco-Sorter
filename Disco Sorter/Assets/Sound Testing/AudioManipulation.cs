using UnityEngine;
using UnityEngine.UI;

public class AudioManipulation : MonoBehaviour
{
    [HideInInspector]
    public float time = 0f;                 // Zmienna opisująca porządany moment w piosence
    [HideInInspector]
    public bool pausePressed = false;       // Zmienna mówiąca czy został wciśnięty przycisk pauzy

    [SerializeField]
    private Slider slider;                   // Zmienna opisująca slider
    [SerializeField]
    private Text timeText;                   // Tekst wyświetlający aktualny czas utworu

    private AudioSource a;                  // Zmienna reprezentująca źródło dźwięku
    private bool virtualPause;              // Zmienna mówiąca czy jest włączona wirtualna pauza*
    private float clampedLength;            // Zmienna opisująca porządany moment w piosence w przedziale <0; 1>
    private string clipLength;

    // * - wirtualna pauza - pauza piosenki mogąca pojawić się bez wciśnięcia przycisku pauzy (bo wymaga tego edytor do niektórych celów)

    /// Pobranie źródła dźwięku, przewinięcie źródła dźwięku do początku, wciśnięcie pauzy oraz pauzy wirtualnej ///
    void Start()
    {
        a = GetComponent<AudioSource>();
        a.time = time;
        pausePressed = true;
        virtualPause = true;

        // Czas trwania utworu przedstawiony w postaci string i ustawienie początkowego czasu
        string minutes = Mathf.Floor(a.clip.length / 60).ToString("00");
        string seconds = Mathf.Floor(a.clip.length % 60).ToString("00");
        clipLength = $"{minutes}:{seconds}";
        TimeTextUpdate();
    }

    /// Funkcja wykonująca co klatkę najważniejsze operacje ///
    void Update()
    {
        Clamp();
        Slider();
        OnClipEnd();
        if (!pausePressed)
            TimeTextUpdate();
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



    private void TimeTextUpdate()
    {
        string minutes = Mathf.Floor(a.time / 60).ToString("00");
        string seconds = Mathf.Floor(a.time % 60).ToString("00");
        timeText.text = $"{minutes}:{seconds} / {clipLength}";
    }

    /// Funkcje odpowiedzialne za przewijanie o konkretną liczbę sekund ///
    /// UWAGA! Zdaję sobie sprawę, że brzydko to wygląda, chciałem to zapisać bez redundancji kodu, ale te fukncje muszą działać ///
    /// z przyciskami Unity, dlatego niemożliwe jest np. podesłanie jako argumentów funkcji w nagłówku jakichś zmiennych. Sorry :( </3 ///

    void Jump(bool forward, float timestep)
    {
        switch (forward)
        {
            case true:
                if (a.time < a.clip.length && a.time < a.clip.length - timestep)
                    a.time += timestep;
                else
                    a.time = 0f;
                return;

            case false:
                if (a.time < a.clip.length && a.time > timestep)
                    a.time -= timestep;
                else
                    a.time = 0f;
                return;
        }
    }

    public void Back(float amountOfTime)
    {
        if (pausePressed)
        {
            Play();
            Jump(false, amountOfTime);
            Pause();
        }
        else
            Jump(false, amountOfTime);
    }

    public void Forward(float amountOfTime)
    {
        if (pausePressed)
        {
            Play();
            Jump(true, amountOfTime);
            Pause();
        }
        else
            Jump(true, amountOfTime);
    }
}
