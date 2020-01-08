﻿using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public int netDensity = 1;                              // Gęstość siatki - WIELOKROTNOŚĆ BPM
    public int BPM;

    [HideInInspector]
    public GameObject[] entityArray;                        // Tablica wszystkich utworzonych obiektów
    [HideInInspector]
    public double[] entityEndTime;                          // Tablica przechowująca czasy końcowe poszczególnych obiektów
    [HideInInspector]
    public int entitiesAmount;                              // Ilość obiektów ustalana na podstawie długości piosenki (w sekundach) i ilości sześcianów na sekundę
    [HideInInspector]
    public string songName;
    [HideInInspector]
    public float step;                                      // Długość trwania jednej kratki

    [SerializeField]
    private GameObject netPosition;                         // Dla ułatwienia. Obiekt, od którego pozycji zaczyna się spawn sześcianów
    [SerializeField]
    private GameObject beatMarker;                          // Prefab znacznika beatu
    [SerializeField]
    private GameObject entity;                              // Prefab obiektu/sześcianu reprezentującego miejsce, w których mogą spawnować się obiekty w grze (różne typy jabłek itd.)
    [SerializeField]
    private GameObject entityCanvas;                        // Obiekt zawierający skrypt EntityMenu

    private AudioClip clip;                                 // Plik audio

    void Start()
    {
        clip = GetComponent<AudioSource>().clip;
        songName = clip.name;

        // Jeśli plik siatki został wczytany, siatka zostanie stworzona z określonymi danymi. Jeśli nie, zostanie zbudowana pusta siatka.
        if (MenuSelectedOption.editorLoaded) LoadNet();
        else BuildNet();
    }

    public void BuildNet()
    {
        DestroyOldNet();
        CreateNet();
        InitializeOther();

        Debug.Log("Ilość obiektów: " + entityArray.Length);
    }

    // Jeśli określony plik siatki został wczytany
    private void LoadNet()
    {
        // Funkcja LoadSong sama w sobie zbuduje siatkę
        MenuSelectedOption.editorLoaded = false;
        GetComponent<SongSaveOrLoad>().LoadSong(MenuSelectedOption.selectedSong);
    }

    // Niszczenie poprzedniej siatki, resetowanie utworu
    private void DestroyOldNet()
    {
        for (int i = 0; i < entitiesAmount; i++)
        {
            Destroy(entityArray[i]);
        }

        Destroy(GameObject.FindGameObjectWithTag("Marker"));

        // Resetowanie piosenki
        gameObject.GetComponent<AudioManipulation>().Restart();
    }

    // Tworzy nową siatkę
    private void CreateNet()
    {
        // Obliczanie obiektów na sekundę, stepów i ilości obiektów nowej siatki
        float entitiesPerSecond = netDensity * BPM / 60f;
        step = 1f / entitiesPerSecond;
        entitiesAmount = (int)Math.Ceiling(clip.length * entitiesPerSecond);

        // Stworzenie tablicy obiektów
        entityArray = new GameObject[entitiesAmount];

        // Stworzenie tablicy czasów końcowych wszystkich kratek
        entityEndTime = new double[entitiesAmount];
        
        Vector3 positionToSpawnEntity = netPosition.transform.position;     // Worldspace pierwszego obiektu siatki
        GameObject createdEntity;                                           // Utworzony właśnie obiekt
        // Spawnowanie obiektów i dodawanie ich do tablicy
        for (int i = 0; i < entitiesAmount; i++)
        {
            createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
            createdEntity.GetComponent<Entity>().entityNumber = i;
            entityArray[i] = createdEntity;
            positionToSpawnEntity.x += entity.transform.lossyScale.x * 1.05f;
        }

        // Pierwszy czas końcowy odpowiada wartości zmiennej step
        entityEndTime[0] = step;

        // Dodawanie do tablicy wartości o czasie końcowym każdej kratki
        for (int i = 1; i < entitiesAmount; i++)
        {
            entityEndTime[i] = Math.Round(entityEndTime[i - 1] + step, 3);
        }
    }

    // Inicializuje pozostałe skrypty, które wymagają do działania danych z tego skryptu
    private void InitializeOther()
    {
        float BPMstep = 1 / (BPM / 60f);

        entityCanvas.GetComponent<EntityMenu>().Initialization();
        GetComponent<EntityCurrentTimeHighlight>().Initialization(entityArray, entityEndTime, entitiesAmount, step);
        GetComponent<DrawWaveForm>().Waveform(); // Wyrenderowanie i synchronizacja waveformu
        MarkBeats(BPMstep);
    }

    // Tworzenie znaczników, które wskazują, który obiekt w siatce odpowiada beatowi
    private void MarkBeats(float BPMstep)
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
