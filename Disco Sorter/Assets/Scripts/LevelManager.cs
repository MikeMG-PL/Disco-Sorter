using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector()]
    public int levelIndex = 0;
    [HideInInspector()]
    public List<string> LevelList = new List<string>();
    public AudioManipulation songController;
    LevelParameters level;
    int iterator = 0, iterator2 = 0;


    float spawnTime, timer; bool timerStarted = false;

    Vector3 A, B, C;
    public Transform TransformOfB;
    public GameObject testBallPrefab;

    public Queue<GameObject> spawnPipeline = new Queue<GameObject>();


    void LoadLevel()
    {
        level = GetComponent<LevelParameters>();

        if (timerStarted && iterator == 0)
        {
            GetComponent<LoadToScene>().LoadSong(levelIndex);
            SetPipeline();
            iterator++;
        }
        else if (timerStarted)
        {
            timer += Time.deltaTime;
            PlayMusic();
            SpawnObjects();
        }
        if (Input.GetKeyDown(KeyCode.Return) && levelIndex < LevelList.Count - 1)
        {
            timerStarted = true;
            Calculations();
            //Instantiate(testBallPrefab, C, Quaternion.identity);
        }
    }

    void Calculations()
    {
        var b = (Mathf.Pow(level.rollTime, 2) * Mathf.Abs(Physics.gravity.y) * Mathf.Sin(0.174532925f)) / (2 * (Mathf.Pow(Mathf.Sin(0.174532925f), 2) + 1) * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2)));
        var h = (b * Mathf.Sin(0.174532925f)) / Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2));

        B = TransformOfB.position;
        A = new Vector3 { x = B.x, y = B.y, z = B.z + b };
        C = new Vector3 { x = A.x, y = A.y + h, z = A.z };
    }

    void Start()
    {
    }

    void PlayMusic()
    {
        if (timer >= level.margin - 0.01f && timer <= level.margin + 0.01f && !songController.a.isPlaying)
            songController.a.Play();
    }

    void SetPipeline()
    {
        for (int i = 0; i < level.spawnPipeline.Count; i++)
        {
            spawnPipeline.Enqueue(level.spawnPipeline[i]);
        }
    }

    void SpawnObjects()
    {
        if (songController.a.isPlaying && iterator2 < level.spawnPipeline.Count)
            spawnTime = level.spawnPipeline[iterator2].GetComponent<ObjectParameters>().spawnTime;

        if (timer >= spawnTime - 0.01f && timer <= spawnTime + 0.01f && timerStarted)
        {
            Instantiate(testBallPrefab, C, Quaternion.identity); // 
            //spawnPipeline.Dequeue();
            iterator2++;
        }
        Debug.Log(timer + " | SPAWNTIME: " +  spawnTime);
    }

    void RythmCheck()
    {

    }

    void Update()
    {
        LoadLevel();
        //Debug.Log(timer);
        // if (timerStarted)
        {
            //timer += Time.deltaTime; testing
            //PlayMusic();
        }

        //if (Input.GetKeyDown(KeyCode.Return)) // START
        {
            //  timerStarted = true;
            //  SpawnObjects();
            //  Instantiate(testBallPrefab, C, Quaternion.identity); // testing
        }



    }
}
