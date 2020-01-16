using UnityEngine;

public class EntityCurrentTimeHighlight : MonoBehaviour
{
    public Color highlightColor;                             // Kolor obiektu, który odpowiada aktualnemu czasowi pliku audio

    private AudioSource audioSource;
    private GameObject[] entityArray;
    private double[] entityEndTime;                         // Tablica przechowująca czasy końcowe poszczególnych obiektów
    private int entitiesAmountInColumn;
    private int entityNumber;                               // Numer obiektu odpowiadającego danemu granemu czasowi pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniemu granemu czasowi pliku audio
    private float currentTime;                              // Aktualny czas granego audio
    private float step;                                     // Długość trwania jednej kratki

    public GameObject currentEntity;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        SetCurrentEntity();
        ChangeHighlightedObject();
        currentEntity = entityArray[entityNumber];
    }

    // Klasa wymaga tablicy entities, które będzie podświetlać i pozostałych zmiennych
    public void Initialization(GameObject[] gameObjects, double[] endTimes, int amount, float editorNetstep)
    {
        entityArray = gameObjects;
        entityEndTime = endTimes;
        entitiesAmountInColumn = amount;
        step = editorNetstep;
    }

    // Ustala, który obiekt odpowiada aktualnemu czasowi piosenki
    void SetCurrentEntity()
    {
        previousEntityNumber = entityNumber;

        // Aktualny czas utworu, jeśli pauza jest aktywna, czas jest brany ze skryptu AudioManipulation
        if (!gameObject.GetComponent<AudioManipulation>().pausePressed)
            currentTime = audioSource.time;
        else
            currentTime = gameObject.GetComponent<AudioManipulation>().time;

        // Pętla określająca numer kratki na bazie czasu piosenki
        if (currentTime <= step)
        {
            entityNumber = 0;
        }

        else
        {
            for (int i = 1; i < entitiesAmountInColumn; i++)
            {
                if (entityEndTime[i] >= currentTime)
                {
                    entityNumber = i;
                    break;
                }
            }
        }
    }

    // Zmienanie koloru obiektu odpowiadającemu aktualnemu czasowi piosenki na highlightColor i poprzednio wyróżnionego na zwykły
    void ChangeHighlightedObject()
    {
        // Jeśli tablica obiektów została już stworzona, i jeśli nastąpiła zmiana obiektu odpowiadającego aktualnemu czasowi pliku audio lub aktywna jest pauza
        if (entityArray != null && (previousEntityNumber != entityNumber || gameObject.GetComponent<AudioManipulation>().pausePressed))
        {
            //if (entityNumber == entitiesAmount) entityNumber--;
            entityArray[previousEntityNumber].GetComponent<Entity>().ChangeColor();
            entityArray[entityNumber].GetComponent<Renderer>().material.color = highlightColor;
        }
    }
}
