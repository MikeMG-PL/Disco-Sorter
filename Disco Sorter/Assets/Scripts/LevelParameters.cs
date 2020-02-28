using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelParameters : MonoBehaviour
{
    ///////////////////////// PREFABY OBIEKTÓW /////////////////////////

    public GameObject apple, rottenApple, disco, release;
    [HideInInspector()]
    public GameObject queueDispenser;

    ///////////////////////// SPAWN PIPELINE, CZYLI LISTA GAMEOBJECTÓW DO WYSPAWNOWANIA /////////////////////////

    public List<GameObject> preSpawnPipeline = new List<GameObject>();
    public List<GameObject> spawnPipeline = new List<GameObject>();

    [Header("---------------------------------------------------------------------")]

    ///////////////////////// DANE WCZYTYWANE Z PLIKU /////////////////////////

    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public List<int> action = new List<int>();

    [Tooltip("Dla tego elementu listy, dla tego ZŁAPANIA, na tej kratce, połączone z nim WYPUSZCZENIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedReleaseEN = new List<int>();

    [Tooltip("Dla tego elementu listy, dla WYPUSZCZENIA, na tej kratce, połączone z nim ZŁAPANIE jest na kratce o ID... (podanym w polu)")]
    public List<int> linkedCatchEN = new List<int>();

    public int BPM, netDensity;
    public float clipLength;

    [Header("---------------------------------------------------------------------")]

    ///////////////////////// DANE WYLICZANE NA BAZIE TYCH Z PLIKU /////////////////////////
    ///// WSZYSTKIE DANE Z UWZGLĘDNIENIEM MARGINESU NA TOCZENIE I TOLERANCJĘ UDERZENIA /////

    public float margin = 10f;                                // Początkowy czas bez żadnych akcji, margines przed piosenką

    public float tolerance = 0.24f;                           /* Tolerancja na reakcję gracza (o ile później lub wcześniej może wykonać określoną akcję i zostanie ona zaliczona)
                                                                 Najlepiej niech step < tolerance < margin */

    public float rollTime = 2;                                // Ile czasu zajmuje obiektom sturlanie się (z równi pochyłej wyliczy się wtedy moment spawnu obiektu) (rolltime < margin)

    public List<float> spawnTime = new List<float>();         // Moment spawnu.

    public List<float> actionTime = new List<float>();        /* Moment, w którym idealnie do rytmu powinniśmy wykonać akcję dla konkretnego elementu. Jeśli akcja
                                                                 to Catch... release, później brany jest linkedReleaseTime dla danego elementu i oczekuje się na akcję 
                                                                 RELEASE w momencie podanego floata linkedReleaseTime (+/- tolerance) i sprawdza się, czy wypuszczono
                                                                 korespondujący z nim linkedCatchTime */

    public List<float> actionStartTime = new List<float>();   // Początek wykonania akcji z uwzględnieniem tolerancji

    public List<float> actionEndTime = new List<float>();     // Koniec wykonania akcji z uwzględnieniem tolerancji

    public List<float> linkedReleaseTime = new List<float>(); /* Czas wypuszczenia trzymanego obiektu w ramach akcji Catch... release. Element reprezentuje ID obiektu,
                                                                 pole - czas jego wypuszczenia. */

    public List<float> linkedCatchTime = new List<float>();   /* Czas złapania wypuszczanego obiektu w ramach akcji Catch... release. Element reprezentuje ID kratki z akcją release,
                                                                 pole - czas złapania obiektu. */

    public void Calculations()
    {
        float entitiesPerSecond = netDensity * BPM / 60f;                                   // Ilość pojawiających się obiektów na sekundę
        float step = 1f / entitiesPerSecond;                                                // Jednostkowy krok pomiędzy obiektami
        int entitiesAmountInColumn = (int)Math.Ceiling(clipLength * entitiesPerSecond);     // Ilość obiektów w jednej wczytanej tablicy

        /// !!! UWAGA !!! - DO ZROBIENIA: obliczenia poniżej zrobić później tylko na zmiennych w celu większej dokładności ///

        // Trochę MaTeMaTyKi: sin 10 = 0,176 => h/b = 0,176 => h = 0,176b => h^2 = 0,030976 * b^2; 2 * h^2 = 0,061952 * b^2;
        // Ze wzoru na czas toczenia się obiektu po równi wyprowadzamy wartość boku b trójkąta prostokątnego podstawy równi. Kąt równi na razie jest stały i wynosi 10 stopni.

        float b = (Mathf.Pow(rollTime, 2) * Physics.gravity.y) / 6.034f;                    // Wartość 6.034 wynika z twierdzenia Pitagorasa i sinusa kąta nachylenia równi (10 st.)
        float h = b * 0.176f;                                                               // Wysokość równi

        /// WAŻNA UWAGA! Równia ma mieć rozmiary większe od tych tutaj wyliczanych (kąt musi się zgadzać). ///
        /// W rzeczywistości obliczamy tutaj dokładne miejsce spawnu jabłka na równi ///
        /// Odległość, którą pokonuje jabłko od miejsca spawnu do miejsca podniesienia jest w stałej relacji z czasem spawnu ///

        ///OBLICZANIE CZASÓW:///
        // Każda akcja dzieje się na początku danej kratki.
        // Tolerancja w takim razie będzie uwzględniała czas "ujemny", kiedy akcja zostanie wykonana przed właściwym czasem piosenki
        // Nie ma to znaczenia. Dodana zostanie lista "pipelineTime", która określa czas akcji z marginesem (właściwe nagranie zostaje odtworzone po czasie margin,
        // przez co czas akcji nigdy nie będzie ujemny. Tolerance < margin)

        Debug.Log("entitiesAmountInColumn: " + entitiesAmountInColumn);

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < entitiesAmountInColumn; i++)
            {
                // Czas akcji dla kratki o określonym ID
                actionTime.Add(0);
                actionTime[i + entitiesAmountInColumn * j] = step * i + margin;

                actionStartTime.Add(0);
                actionStartTime[i + entitiesAmountInColumn * j] = (step * i) - tolerance + margin;

                actionEndTime.Add(0);
                actionEndTime[i + entitiesAmountInColumn * j] = (step * i) + tolerance + margin;

                spawnTime.Add(0);
                spawnTime[i + entitiesAmountInColumn * j] = actionTime[i + entitiesAmountInColumn * j] - rollTime;
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////
    /// TU SKOŃCZYŁEM: JESTEM BLISKO. SORTOWANIE DZIAŁA, ALE TRZEBA DOKOŃCZYĆ SPAWNPIPELINE


    // TRZEBA PRZESORTOWAĆ KOLEJKĘ CHRONOLOGICZNIE, WEDŁUG CZASU SPAWNU - ROSNĄCO
    public void ConvertToPipeline()
    {
        for (int i = 0; i <= entityType.Count - 1; i++)
        {
            switch (entityType[i])
            {
                case 0:
                    break;

                case 1:
                    queueDispenser = apple;
                    SetDispenser(i);
                    preSpawnPipeline.Add(queueDispenser);
                    //Debug.Log(queueDispenser.GetComponent<ObjectParameters>().spawnTime + " // jabłko");
                    break;

                case 2:
                    queueDispenser = rottenApple;
                    SetDispenser(i);
                    preSpawnPipeline.Add(queueDispenser);
                    //Debug.Log(queueDispenser.GetComponent<ObjectParameters>().spawnTime + " // zgniłe jabłko");
                    break;

                case 3:
                    queueDispenser = disco;
                    SetDispenser(i);
                    preSpawnPipeline.Add(queueDispenser);
                    //Debug.Log(queueDispenser.GetComponent<ObjectParameters>().spawnTime + " // kula disco");
                    break;

                default:
                    break;
            }
        }
        //spawnPipeline = preSpawnPipeline.OrderBy(preSpawnPipeline => preSpawnPipeline.GetComponent<ObjectParameters>().spawnTime).ToList();

        for (int i = 0; i < preSpawnPipeline.Count; i++)
        {
            Debug.Log(preSpawnPipeline[i].GetComponent<ObjectParameters>().spawnTime);
        }



        void SetDispenser(int j)
        {
            queueDispenser.GetComponent<ObjectParameters>().actionTime = actionTime[j];
            queueDispenser.GetComponent<ObjectParameters>().actionStartTime = actionStartTime[j];
            queueDispenser.GetComponent<ObjectParameters>().actionEndTime = actionEndTime[j];
            //queueDispenser.GetComponent<ObjectParameters>().linkedReleaseTime = linkedReleaseTime[j];
            //queueDispenser.GetComponent<ObjectParameters>().linkedCatchTime = linkedCatchTime[j];
            queueDispenser.GetComponent<ObjectParameters>().spawnTime = spawnTime[j];
            queueDispenser.GetComponent<ObjectParameters>().type = entityType[j];
            queueDispenser.GetComponent<ObjectParameters>().color = color[j];
            queueDispenser.GetComponent<ObjectParameters>().action = action[j];
            queueDispenser.GetComponent<ObjectParameters>().ID = j;
        }



        // sprawdzaj pierwsze 8 elementów listy
        void SpawnElements()
        {
            ; // WIP
        }

        void RythmCheck()
        {
            ; // WIP
        }
    }
}


/*entitiesOverallAmount = entitiesAmountInColumn * 4;
        entityArray = new GameObject[entitiesOverallAmount];                // Stworzenie tablicy obiektów 
        entityEndTime = new double[entitiesAmountInColumn];                 // Stworzenie tablicy czasów końcowych wszystkich kratek w kolumnie

        Vector3 positionToSpawnEntity = netPosition.transform.position;     // Worldspace pierwszego obiektu siatki
        GameObject createdEntity;                                           // Utworzony właśnie obiekt
        int overallNumber;

        // Spawnowanie obiektów i dodawanie ich do tablicy
        for (int j = 0; j < columnsAmount; j++)
        {
            if (j != 0)
                positionToSpawnEntity = new Vector3(netPosition.transform.position.x, netPosition.transform.position.y, positionToSpawnEntity.z - 0.15f);

            for (int i = 0; i < entitiesAmountInColumn; i++)
            {
                overallNumber = j * entitiesAmountInColumn + i;
                createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
                createdEntity.GetComponent<Entity>().entityNumber = overallNumber;
                entityArray[overallNumber] = createdEntity;
                positionToSpawnEntity.x += entity.transform.lossyScale.x * 1.05f;
            }
        }

        // Dodawanie do tablicy wartości o czasie końcowym każdej kratki, potrzebna tylko jedna kolumna
        for (int i = 0; i < entitiesAmountInColumn; i++)
        {
            entityEndTime[i] = Math.Round(step * (i + 1), 3);
        }*/
