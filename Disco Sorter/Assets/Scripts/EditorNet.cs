using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public GameObject entity;                               // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    public GameObject positionForEntities;                  // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów

    private AudioClip clip;                                 // Plik audio
    private AudioSource audioSource;
    private Vector3 positionToSpawnEntity;                  // Pozycja spawnu obiektu
    private GameObject createdEntity;                       // Utworzony właśnie obiekt
    private GameObject[] entityArray;                       // Tablica wszystkich utworzonych obiektów
    private double currentTime;                             // Aktualny czas granego audio 
    private int entityNumber;                               // Numer obiektu odpowiadającego aktualnie granej sekundzie pliku audio
    private int previousEntityNumber;                       // Numer obiektu odpowiadającego poprzedniej granej sekundzie pliku audio

    // Jeden sześcian odpowiada jednej sekundzie
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
        positionToSpawnEntity = positionForEntities.transform.position;

        // Ustalenie wielkość tablicy na podstawie długości piosenki (w sekundach)
        entityArray = new GameObject[Convert.ToInt32(clip.length) + 1];

        // Spawnowanie sześcianów
        for (int i = 0; i < clip.length; i++)
        {
            createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
            createdEntity.GetComponent<Entity>().EntityNumber = i;
            entityArray[i] = createdEntity;
            positionToSpawnEntity.x += entity.transform.lossyScale.x * 1.05f;
        }
    }


    void Update()
    {
        currentTime = audioSource.time;                 // Aktualny czas utworu 
        previousEntityNumber = entityNumber;            // Poprzednio wyróżniony obiekt
        entityNumber = Convert.ToInt32(currentTime);    // Aktualnie wyróżniony obiekt

        ChangeHighlightedObject();
    }

    // Zmienanie koloru obiektu wyróżnionego na zielony i poprzednio wyróżnionego na zwykły
    void ChangeHighlightedObject()
    {
        if (entityArray[0] != null)
        {
            entityArray[previousEntityNumber].GetComponent<Renderer>().material.color = Color.white;
            entityArray[entityNumber].GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
