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
    GameObject queueDispenser;

    ///////////////////////// SPAWN PIPELINE, CZYLI LISTA GAMEOBJECTÓW DO WYSPAWNOWANIA /////////////////////////
    //[HideInInspector()]
    public List<GameObject> spawnPipeline = new List<GameObject>();



    ///////////////////////// DANE WCZYTYWANE Z PLIKU /////////////////////////

    [HideInInspector()]
    public List<EntityType> entityType = new List<EntityType>();
    [HideInInspector()]
    public List<EntityColour> color = new List<EntityColour>();
    [HideInInspector()]
    public List<EntityAction> action = new List<EntityAction>();

    // Dla tego elementu listy, dla tego ZŁAPANIA, na tej kratce, połączone z nim WYPUSZCZENIE jest na kratce o ID... (podanym w polu)
    [HideInInspector()]
    public List<int> linkedReleaseEN = new List<int>();

    // Dla tego elementu listy, dla WYPUSZCZENIA, na tej kratce, połączone z nim ZŁAPANIE jest na kratce o ID... (podanym w polu)
    [HideInInspector()]
    public List<int> linkedCatchEN = new List<int>();

    [HideInInspector()]
    public int BPM, netDensity;
    [HideInInspector()]
    public float clipLength;

    ///////////////////////// DANE WYLICZANE NA BAZIE TYCH Z PLIKU /////////////////////////
    ///// WSZYSTKIE DANE Z UWZGLĘDNIENIEM MARGINESU NA TOCZENIE I TOLERANCJĘ UDERZENIA /////

    public int entitiesAmountInColumn;

    [Header("------------------------------------------------------------------")]
    public float margin = 10f;                                // Początkowy czas bez żadnych akcji, margines przed piosenką

    public float tolerance = 0.24f;                           /* Tolerancja na reakcję gracza (o ile później lub wcześniej może wykonać określoną akcję i zostanie ona zaliczona)
                                                                 Najlepiej niech step < tolerance < margin */

    public float rollTime = 2;                                // Ile czasu zajmuje obiektom sturlanie się (z równi pochyłej wyliczy się wtedy moment spawnu obiektu) (rolltime < margin)

    List<float> spawnTime = new List<float>();                // Moment spawnu.

    List<float> actionTime = new List<float>();               /* Moment, w którym idealnie do rytmu powinniśmy wykonać akcję dla konkretnego elementu. Jeśli akcja
                                                                 to Catch... release, później brany jest linkedReleaseTime dla danego elementu i oczekuje się na akcję 
                                                                 RELEASE w momencie podanego floata linkedReleaseTime (+/- tolerance) i sprawdza się, czy wypuszczono
                                                                 korespondujący z nim linkedCatchTime */

    List<float> actionStartTime = new List<float>();          // Początek wykonania akcji z uwzględnieniem tolerancji

    List<float> actionEndTime = new List<float>();            // Koniec wykonania akcji z uwzględnieniem tolerancji

    List<float> linkedReleaseTime = new List<float>();        /* Czas wypuszczenia trzymanego obiektu w ramach akcji Catch... release. Element reprezentuje ID obiektu,
                                                                 pole - czas jego wypuszczenia. */

    List<float> linkedCatchTime = new List<float>();          /* Czas złapania wypuszczanego obiektu w ramach akcji Catch... release. Element reprezentuje ID kratki z akcją release,
                                                                 pole - czas złapania obiektu. */

    

    public void Calculations()
    {
        var entitiesPerSecond = netDensity * BPM / 60f;                                   // Ilość pojawiających się obiektów na sekundę
        var step = 1f / entitiesPerSecond;                                                // Jednostkowy krok pomiędzy obiektami
        entitiesAmountInColumn = (int)Math.Ceiling(clipLength * entitiesPerSecond);       // Ilość obiektów w jednej wczytanej tablicy

        // Trochę MaTeMaTyKi: W wyniku przekształcenia wzoru na czas toczenia, w zależności od zmiennej rollTime jesteśmy w stanie wyznaczyć położenie turlającego się obiektu

        

        ///OBLICZANIE CZASÓW:///
        // Każda akcja dzieje się na początku danej kratki.
        // Tolerancja w takim razie będzie uwzględniała czas "ujemny", kiedy akcja zostanie wykonana przed właściwym czasem piosenki.
        // Wszystkie czasy zawierają już ten margines (margin).

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

    public void ConvertToPipeline()
    {
        var pos = new Vector3 { x = 100, y = 100, z = 100 };
        int row = 0, column = 0, index;
        while (row < entitiesAmountInColumn)
        {
            bool addToPipeline = true; // Czy dodać element do pipeline, domyślnie tak
            index = entitiesAmountInColumn * column + row;
            switch (entityType[index])
            {
                case EntityType.Apple:
                    queueDispenser = Instantiate(apple, pos, Quaternion.identity);
                    break;

                case EntityType.RottenApple:
                    queueDispenser = Instantiate(rottenApple, pos, Quaternion.identity);
                    break;

                case EntityType.Disco:
                    queueDispenser = Instantiate(disco, pos, Quaternion.identity);
                    break;

                default:
                    addToPipeline = false;
                    break;
            }

            if (addToPipeline)
            {
                SetParameters(index);
                spawnPipeline.Add(queueDispenser);
            }

            if (column == 3)
            {
                column = -1;
                row++;
            }

            column++;
        }
    }

    void SetParameters(int j)
    {
        ObjectParameters q = queueDispenser.GetComponent<ObjectParameters>();

        q.actionTime = actionTime[j];
        q.actionStartTime = actionStartTime[j];
        q.actionEndTime = actionEndTime[j];
        q.spawnTime = spawnTime[j];
        q.type = entityType[j];
        q.color = color[j];
        q.action = action[j];
        q.ID = j;

        if(q.type == EntityType.Apple)
        {
            switch(q.color)
            {
                case EntityColour.Red:
                    break;

                case EntityColour.Green:
                    q.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = q.gameObject.GetComponent<MeshRenderer>().material;
                    break;

                default:
                    break;
            }
        }

        // Niedokończone: w przypadku, gdy akcja to "catch... release" - należy ustawić te parametry
        //q.linkedReleaseTime = linkedReleaseTime[j];
        //q.linkedCatchTime = linkedCatchTime[j];
    }
}
