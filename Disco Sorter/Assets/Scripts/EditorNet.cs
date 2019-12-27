using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public Camera camera;
    public GameObject entity;                               // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    public GameObject[] entityArray;                        // Tablica wszystkich utworzonych obiektów
    public GameObject positionForEntities;                  // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów
    public int entitiesPerSecond = 2;                       // Ile entities/obiektów może mieścić się w jednej sekundzie piosenki

    private AudioClip clip;                                 // Plik audio
    private AudioSource audioSource;
    private Vector3 positionToSpawnEntity;                  // Pozycja spawnu obiektu
    private GameObject createdEntity;                       // Utworzony właśnie obiekt
    private double currentTime;                             // Aktualny czas granego audio 
    private int entityNumber;                               // Numer obiektu odpowiadającego danemu granemu czasowi pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniemu granemu czasowi pliku audio
    private int entitiesAmount;                             // Ilość obiektów ustalana na podstawie długości piosenki (w sekundach) i ilości sześcianów na sekundę
    private float cameraSpeed = 3.5f;
    private float scroll;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
        positionToSpawnEntity = positionForEntities.transform.position;
        entitiesAmount = Convert.ToInt32(clip.length * entitiesPerSecond);

        // Ustalenie wielkość tablicy
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
        // Aktualny czas utworu, jeśli pauza jest aktywna, czas jest brany ze skryptu AudioManipulation
        if (!gameObject.GetComponent<AudioManipulation>().pausePressed)
            currentTime = audioSource.time;
        else
            currentTime = gameObject.GetComponent<AudioManipulation>().time;

        previousEntityNumber = entityNumber;                                // Poprzednio wyróżniony obiekt
        entityNumber = (int)(currentTime * entitiesPerSecond);              // Aktualnie wyróżniony obiekt

        ChangeHighlightedObject();

        scroll = Input.GetAxis("Mouse ScrollWheel");
        camera.transform.Translate(0, 0, scroll * cameraSpeed, Space.Self);
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
