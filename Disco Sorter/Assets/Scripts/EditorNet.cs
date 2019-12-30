using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public GameObject entity;                               // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    public GameObject[] entityArray;                        // Tablica wszystkich utworzonych obiektów
    public GameObject positionForEntities;                  // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów
    public int entitiesPerSecond = 2;                       // Ile entities/obiektów może mieścić się w jednej sekundzie piosenki

    private AudioClip clip;                                 // Plik audio
    private AudioSource audioSource;
    private Vector3 positionToSpawnEntity;                  // Pozycja spawnu obiektu
    private GameObject createdEntity;                       // Utworzony właśnie obiekt
    private float currentTime;                             // Aktualny czas granego audio 
    private int entityNumber;                               // Numer obiektu odpowiadającego danemu granemu czasowi pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniemu granemu czasowi pliku audio
    private int entitiesAmount;                             // Ilość obiektów ustalana na podstawie długości piosenki (w sekundach) i ilości sześcianów na sekundę

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
        positionToSpawnEntity = positionForEntities.transform.position;
        entitiesAmount = (int)Math.Round(clip.length) * entitiesPerSecond;
        entityArray = new GameObject[entitiesAmount];

        // Spawnowanie sześcianów i dodawanie ich do tablicy
        for (int i = 0; i < entitiesAmount; i++)
        {
            createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
            createdEntity.GetComponent<Entity>().entityNumber = i;
            entityArray[i] = createdEntity;
            positionToSpawnEntity.x += entity.transform.lossyScale.x * 1.05f;
        }
    }

    void Update()
    {
        SetCurrentEntity();
        ChangeHighlightedObject();
    }

    // Ustala, który obiekt odpowiada aktualnemu czasowi piosenki
    void SetCurrentEntity()
    {
        // Aktualny czas utworu, jeśli pauza jest aktywna, czas jest brany ze skryptu AudioManipulation
        if (!gameObject.GetComponent<AudioManipulation>().pausePressed)
            currentTime = audioSource.time;
        else
            currentTime = gameObject.GetComponent<AudioManipulation>().time;

        // Trochę ***MaTeMaTyKi***, która nie wiem czy jest poprawna, ale zaokrąglanie liczb sprawiło tutaj spory problem.
        float decimals = currentTime - (int)currentTime;

        previousEntityNumber = entityNumber;                               // Poprzednio wyróżniony obiekt
        // Aktualnie wyróżniony obiekt
        if (decimals >= 0.5) entityNumber = (int)Math.Round(currentTime) * entitiesPerSecond - 1;
        else entityNumber = (int)Math.Round(currentTime) * entitiesPerSecond;

        //Debug.Log(entityNumber);
    }

    // Zmienanie koloru obiektu odpowiadającemu aktualnemu czasowi piosenki na zielony i poprzednio wyróżnionego na zwykły
    void ChangeHighlightedObject()
    {
        if (entityArray[0] != null)
        {
            if (entityNumber == entitiesAmount) entityNumber--;
            entityArray[previousEntityNumber].GetComponent<Renderer>().material.color = Color.white;
            entityArray[entityNumber].GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
