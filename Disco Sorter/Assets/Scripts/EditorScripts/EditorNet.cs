using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public GameObject beatMarker;                           // Prefab znacznika beatu
    public GameObject entity;                               // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    public GameObject[] entityArray;                        // Tablica wszystkich utworzonych obiektów
    double[] entityEndTime;                                 // Tablica przechowująca czasy końcowe poszczególnych obiektów
    public GameObject positionForEntities;                  // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów
    public string songName;
    public int BPM;
    float entitiesPerSecond;                                // Ile entities/obiektów może mieścić się w jednej sekundzie piosenki
    float step, BPMstep;                                             // Długość trwania jednej kratki
    public int netDestiny = 1;                                         // Gęstość siatki - WIELOKROTNOŚĆ BPM

    private AudioClip clip;                                 // Plik audio
    private AudioSource audioSource;
    private Vector3 positionToSpawnEntity;                  // Pozycja spawnu obiektu
    private GameObject createdEntity;                       // Utworzony właśnie obiekt
    private float currentTime;                              // Aktualny czas granego audio 
    private int entityNumber;                               // Numer obiektu odpowiadającego danemu granemu czasowi pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniemu granemu czasowi pliku audio
    private Color highlightColor = Color.blue;              // Kolor obiektu, który odpowiada aktualnemu czasowi pliku audio
    private int entitiesAmount;                             // Ilość obiektów ustalana na podstawie długości piosenki (w sekundach) i ilości sześcianów na sekundę

    void Awake()
    {
        entitiesPerSecond = (netDestiny * BPM) / 60f;
        step = 1f / entitiesPerSecond;
        BPMstep = 1 / (BPM / 60f);

        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
        songName = clip.name;

        positionToSpawnEntity = positionForEntities.transform.position;
        entitiesAmount = (int)(Math.Ceiling(clip.length * entitiesPerSecond));
        entityArray = new GameObject[entitiesAmount];
        // Stworzenie tablicy czasów końcowych wszystkich kratek
        entityEndTime = new double[entitiesAmount];

        // Spawnowanie sześcianów i dodawanie ich do tablicy
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
        MarkBeats();
    }

    void Update()
    {
        SetCurrentEntity();
        ChangeHighlightedObject();
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

    void MarkBeats()
    {
        for (int i = 0; i < entitiesAmount; i++)
        {
            //if (i == 0)
            //Instantiate(beatMarker, new Vector3(entityArray[i].transform.position.x, entityArray[i].transform.position.y, entityArray[i].transform.position.z + 0.075f), Quaternion.identity);

            if (entityEndTime[i] % BPMstep <= 0.01 && (i + netDestiny - netDestiny / 2 <= entityEndTime.Length - 1))
            {
                //entityArray[i].GetComponent<Renderer>().material.color = Color.blue;
                Instantiate(beatMarker, new Vector3(entityArray[i + netDestiny - (int)Math.Ceiling((float)(netDestiny / 2))].transform.position.x, entityArray[i + netDestiny - (int)Math.Ceiling((float)(netDestiny / 2))].transform.position.y, entityArray[i + netDestiny - (int)Math.Ceiling((float)(netDestiny / 2))].transform.position.z + 0.075f), Quaternion.identity);
            }


        }
    }
}
