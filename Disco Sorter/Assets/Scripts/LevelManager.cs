using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AudioManipulation songController;
    public LevelParameters level;

    float timer; bool timerStarted = false;

    Vector3 A, B, C;
    public Transform TransformOfB;
    public GameObject testBallPrefab;

    public Queue<GameObject> spawnPipeline = new Queue<GameObject>();

    void Calculations()
    {
        var b = (Mathf.Pow(level.rollTime, 2) * Mathf.Abs(Physics.gravity.y) * Mathf.Sin(0.174532925f)) / (2 * (Mathf.Pow(Mathf.Sin(0.174532925f), 2) + 1) * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2)));
        var h = (b * Mathf.Sin(0.174532925f)) / Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2));

        B = TransformOfB.position;
        A = new Vector3 { x = B.x, y = B.y, z = B.z + b };
        C = new Vector3 { x = A.x, y = A.y + h, z = A.z };

        //Instantiate(testBallPrefab, C, Quaternion.identity); // testing
    }

    void Start()
    {
        Calculations();
    }

    void PlayMusic()
    {
        if (timer >= level.margin - 0.01f && timer <= level.margin + 0.01f && !songController.a.isPlaying)
            songController.a.Play();
    }

    void SpawnObjects()
    {
        for(int i = 0; i < level.spawnPipeline.Count; i++)
        {
            spawnPipeline.Enqueue(level.spawnPipeline[i]);
        }

        for()
    }

    void RythmCheck()
    {

    }

    void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
            PlayMusic();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // START
        {
            timerStarted = true;
            SpawnObjects();
        }

        
            
    }
}
