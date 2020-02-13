using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParameters : MonoBehaviour
{
    ///////////////////////// DANE WCZYTYWANE Z PLIKU /////////////////////////
    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public List<int> action = new List<int>();

    [Tooltip("Dla tego elementu listy, dla tego ZŁAPANIA, na tej kratce, połączone z nim WYPUSZCZENIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedReleaseEN = new List<int>();

    [Tooltip("Dla tego elementu listy, dla WYPUSZCZENIA, na tej kratce, połączone z nim ZŁAPANIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedCatchEN = new List<int>();

    public int BPM, netDensity;

    [Header("---------------------------------------------------------------------")]

    ///////////////////////// DANE WYLICZANE NA BAZIE TYCH Z PLIKU /////////////////////////

    public List<float> actionTime = new List<float>();        /* Moment, w którym idealnie do rytmu powinniśmy wykonać akcję dla konkretnego elementu. Jeśli akcja
                                                                 to Catch... release, później brany jest linkedReleaseTime dla danego elementu i oczekuje się na akcję 
                                                                 RELEASE w momencie podanego floata linkedReleaseTime (+/- tolerance) i sprawdza się, czy wypuszczono
                                                                 korespondujący z nim linkedCatchTime */

    public float tolerance;                                   // Tolerancja na reakcję gracza (o ile później lub wcześniej może wykonać określoną akcję i zostanie ona zaliczona)
    public float rollTime;                                    // Ile czasu zajmie określonym obiektom sturlanie się (z równi pochyłej wyliczy się wtedy moment spawnu obiektu)
    public List<float> actionStartTime = new List<float>();   // Początek wykonania akcji z uwzględnieniem tolerancji
    public List<float> actionEndTime = new List<float>();     // Koniec wykonania akcji z uwzględnieniem tolerancji

    public List<float> linkedReleaseTime = new List<float>(); /* Czas wypuszczenia trzymanego obiektu w ramach akcji Catch... release. Element reprezentuje ID obiektu,
                                                                 pole - czas jego wypuszczenia.*/

    public List<float> linkedCatchTime = new List<float>();   /* Czas złapania wypuszczanego obiektu w ramach akcji Catch... release. Element reprezentuje ID kratki z akcją release,
                                                                 pole - czas złapania obiektu. */

    void Calculations()
    {
        /// ALGORYTM ///
        // Dla każdego elementu listy entityType przekonwertuj netDensity, BPM i ID elementu na czas wykonania akcji. Uwzględnij, że masz 4 rzędy!!!
        // Na bieżąco dodawaj wyliczony czas wykonania akcji dla danego elementu do listy actionTime.
        // Odejmij wartość tolerance od actionTime dla danego elementu - dodaj tę wartość dla odpowiedniego elementu do actionStartTime.
        // Dodaj wartość tolerance do actionTime dla danego elementu - dodaj tę wartość dla odpowiedniego elementu do actionEndTime.
        // ... algorytm jeszcze wymaga dokończenia
    }

    void SpawnElements()
    {
        ; // WIP
    }
}