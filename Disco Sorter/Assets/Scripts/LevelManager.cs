using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector()]
    public int levelIndex;
    [HideInInspector()]
    public List<string> LevelList = new List<string>();
    public GameAudioManipulation songController;
    LevelParameters level;
    int iterator;


    float spawnTime, timer; bool timerStarted;

    Vector3 A, B, C;
    public Transform TransformOfB;
    public GameObject testBallPrefab;

    public List<GameObject> spawnPipeline = new List<GameObject>();

    void Start()
    {
        level = GetComponent<LevelParameters>();
        GetComponent<LoadToScene>().LoadSong(levelIndex);
        spawnPipeline = level.spawnPipeline;
        Calculations();

        for(int i = 0; i < spawnPipeline.Count; i++)
        {
            //Debug.Log(spawnPipeline[i].GetComponent<ObjectParameters>().spawnTime);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && levelIndex < LevelList.Count - 1 && !songController.aSrc.isPlaying)
        {
            timerStarted = true;
        }

        if (timerStarted)
        {
            PlayMusic();
            SpawnObjects();
            timer += Time.deltaTime;
        }
    }

    void Calculations()
    {
        var b = (Mathf.Pow(level.rollTime, 2) * Mathf.Abs(Physics.gravity.y) * Mathf.Sin(0.174532925f)) /
                (2 * (Mathf.Pow(Mathf.Sin(0.174532925f), 2) + 1) * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2)));

        var h = (b * Mathf.Sin(0.174532925f)) / Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2));

        B = TransformOfB.position;
        A = new Vector3 { x = B.x, y = B.y, z = B.z + b };
        C = new Vector3 { x = A.x, y = A.y + h, z = A.z };
    }

    void PlayMusic()
    {
        if (timer >= level.margin - 0.01f && timer <= level.margin + 0.01f && !songController.aSrc.isPlaying)
            songController.aSrc.Play();
    }

    void SpawnObjects()
    {
        if (iterator < spawnPipeline.Count)
        {
            spawnTime = spawnPipeline[iterator].GetComponent<ObjectParameters>().spawnTime;

            if (timer >= spawnTime - 0.01f && timer <= spawnTime + 0.01f && timerStarted)
            {
                spawnPipeline[iterator].transform.position = SetRowPosition(id: spawnPipeline[iterator].GetComponent<ObjectParameters>().ID, pos: C);
                spawnPipeline[iterator].transform.rotation = Quaternion.identity;
                spawnPipeline[iterator].GetComponent<Rigidbody>().velocity = Vector3.zero;
                iterator++;
            }
        }
    }

    Vector3 SetRowPosition(int id, Vector3 pos)
    {
        int entitiesInColumn = GetComponent<LevelParameters>().entitiesAmountInColumn;
        Vector3 finalPos = pos;

        if (id < entitiesInColumn)
            return finalPos;

        else if (id < entitiesInColumn * 2)
            finalPos = new Vector3(finalPos.x + 0.5f, finalPos.y, finalPos.z);

        else if (id < entitiesInColumn * 3)
            finalPos = new Vector3(finalPos.x + 1f, finalPos.y, finalPos.z);

        else if (id < entitiesInColumn * 4)
            finalPos = new Vector3(finalPos.x + 1.5f, finalPos.y, finalPos.z);

        return finalPos;
    }
}
