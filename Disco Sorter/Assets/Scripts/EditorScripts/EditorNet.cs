using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public GameObject beatMarker;                           // Prefab znacznika beatu
    public GameObject entity;                               // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    public GameObject entityCanvas;                         // Obiekt zawierający skrypt EntityMenu
    [HideInInspector]
    public GameObject[] entityArray;                        // Tablica wszystkich utworzonych obiektów
    public GameObject positionForEntities;                  // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów
    public int BPM;
    public int entitiesAmount;                             // Ilość obiektów ustalana na podstawie długości piosenki (w sekundach) i ilości sześcianów na sekundę
    public int netDensity = 1;                              // Gęstość siatki - WIELOKROTNOŚĆ BPM
    [HideInInspector]
    public string songName;

    private AudioClip clip;                                 // Plik audio
    private AudioSource audioSource;
    private Color highlightColor = Color.blue;              // Kolor obiektu, który odpowiada aktualnemu czasowi pliku audio
    private double[] entityEndTime;                         // Tablica przechowująca czasy końcowe poszczególnych obiektów
    private float currentTime;                              // Aktualny czas granego audio 
    private float step;                                     // Długość trwania jednej kratki
    private int entityNumber;                               // Numer obiektu odpowiadającego danemu granemu czasowi pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniemu granemu czasowi pliku audio

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
        songName = clip.name;

        BuildNet();
    }

    void Update()
    {
        SetCurrentEntity();
        ChangeHighlightedObject();
    }

    public void BuildNet()
    {
        for (int i = 0; i < entitiesAmount; i++)
        {
            Destroy(entityArray[i]);
        }
        Destroy(GameObject.FindGameObjectWithTag("Marker"));

        // Resetowanie piosenki
        gameObject.GetComponent<AudioManipulation>().Restart();

        // Ustawianie pozycji dla pierwszego obiektu siatki
        Vector3 positionToSpawnEntity = positionForEntities.transform.position;     // Pozycja spawnu obiektu

        // Obliczanie obiektów na sekundę, stepów i ilości obiektów
        float entitiesPerSecond = netDensity * BPM / 60f;
        step = 1f / entitiesPerSecond;
        float BPMstep = 1 / (BPM / 60f);
        entitiesAmount = (int)Math.Ceiling(clip.length * entitiesPerSecond);

        // Stworzenie tablicy obiektów
        entityArray = new GameObject[entitiesAmount];

        // Stworzenie tablicy czasów końcowych wszystkich kratek
        entityEndTime = new double[entitiesAmount];

        GameObject createdEntity;   // Utworzony właśnie obiekt
        // Spawnowanie obiektów i dodawanie ich do tablicy
        for (int i = 0; i < entitiesAmount; i++)
        {
            createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
            createdEntity.GetComponent<Entity>().entityNumber = i;
            entityArray[i] = createdEntity;
            positionToSpawnEntity.x += entity.transform.lossyScale.x * 1.05f;
        }

        // Pierwszy obiekt odpowiada początkowemu czasowi piosenki
        entityArray[0].GetComponent<Renderer>().material.color = highlightColor;

        // Pierwszy czas końcowy odpowiada wartości zmiennej step
        entityEndTime[0] = step;

        // Dodawanie do tablicy wartości o czasie końcowym każdej kratki
        for (int i = 1; i < entitiesAmount; i++)
        {
            entityEndTime[i] = Math.Round(entityEndTime[i - 1] + step, 3);
        }

        entityCanvas.GetComponent<EntityMenu>().Initialization();
        gameObject.GetComponent<AudioManipulation>().Waveform(); // wwyrenderowanie i synchronizacja waveformu
        MarkBeats(BPMstep);

        Debug.Log("Ilość obiektów: " + entityArray.Length);
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
            for (int i = 1; i < entitiesAmount; i++)
            {
                if (entityEndTime[i] >= currentTime)
                {
                    entityNumber = i;
                    break;
                }
            }
        }

        //Debug.Log(entityNumber);
    }

    // Zmienanie koloru obiektu odpowiadającemu aktualnemu czasowi piosenki na zielony i poprzednio wyróżnionego na zwykły
    void ChangeHighlightedObject()
    {
        // Jeśli tablica obiektów została już stworzona, i jeśli nastąpiła zmiana obiektu odpowiadającego aktualnemu czasowi pliku audio lub aktywna jest pauza
        if (entityArray[0] != null && (previousEntityNumber != entityNumber || gameObject.GetComponent<AudioManipulation>().pausePressed))
        {
            //if (entityNumber == entitiesAmount) entityNumber--;
            entityArray[previousEntityNumber].GetComponent<Renderer>().material.color = entityArray[previousEntityNumber].GetComponent<Entity>().GetColor();
            entityArray[entityNumber].GetComponent<Renderer>().material.color = highlightColor;
        }
    }

    void MarkBeats(float BPMstep)
    {
        int num;
        for (int i = 0; i < entitiesAmount; i++)
        {
            //if (i == 0)
            //Instantiate(beatMarker, new Vector3(entityArray[i].transform.position.x, entityArray[i].transform.position.y, entityArray[i].transform.position.z + 0.075f), Quaternion.identity);

            if (entityEndTime[i] % BPMstep <= 0.01 && (i + netDensity - netDensity / 2 <= entityEndTime.Length - 1))
            {
                num = i + netDensity - (int)Math.Ceiling((float)(netDensity / 2));
                //entityArray[i].GetComponent<Renderer>().material.color = Color.blue;
                Instantiate(beatMarker, new Vector3(entityArray[num].transform.position.x, entityArray[num].transform.position.y, entityArray[num].transform.position.z + 0.075f), Quaternion.identity, entityArray[num].transform);
            }
        }
    }
}
