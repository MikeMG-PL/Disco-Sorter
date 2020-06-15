using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject playerObject;
    GameAudioManipulation songController;
    [HideInInspector()]
    static public float timer;

    public int index;           // chosen song from list
    public List<ScriptableObject> buildLevels;
    public List<AudioClip> buildSongs;

    LevelParameters levelParameters;
    Player player;
    int iterator;
    float spawnTime;
    bool timerStarted;

    Vector3 A, B, C;
    public Transform TransformOfB;

    [HideInInspector()]
    public List<GameObject> spawnPipeline = new List<GameObject>();

    private void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 75;
#endif
    }

    private void Start()
    {
        songController = GetComponent<GameAudioManipulation>();
        player = playerObject.GetComponent<Player>();

        if (!songController.aSrc.isPlaying)
        {
            levelParameters = GetComponent<LevelParameters>();
            spawnPipeline = levelParameters.spawnPipeline;
            Calculations();
            timerStarted = true;
        }
    }

    void FixedUpdate()
    {
        if (timerStarted)
        {
            PlayMusic();
            SpawnObjects();
            timer += Time.fixedDeltaTime;
        }

        if (songController.aSrc.isPlaying && Input.GetKeyDown(KeyCode.Return))
        {
            Restart();
        }
    }

    void Calculations()
    {

        var b = (Mathf.Pow(levelParameters.rollTime * 0.83f, 2) * Mathf.Abs(Physics.gravity.y) * Mathf.Sin(0.174532925f)) /
                (2 * (Mathf.Pow(Mathf.Sin(0.174532925f), 2) + 1) * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2)));

        var h = (b * Mathf.Sin(0.174532925f)) / Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2));

        B = TransformOfB.position;
        A = new Vector3 { x = B.x, y = B.y, z = B.z + b };
        C = new Vector3 { x = A.x, y = A.y + h, z = A.z };
    }

    void PlayMusic()
    {
        if (timer >= levelParameters.margin - 0.01f && timer <= levelParameters.margin + 0.01f && !songController.aSrc.isPlaying)
            songController.aSrc.Play();
    }

    void StopMusic()
    {
        if (songController.aSrc.isPlaying)
            songController.aSrc.Stop();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timer = 0;
    }

    void SpawnObjects()
    {
        if (iterator < spawnPipeline.Count)
        {
            Vector3 rot = new Vector3(Random.Range(0, 181), Random.Range(0, 181), Random.Range(0, 181));

            spawnTime = spawnPipeline[iterator].GetComponent<ObjectParameters>().spawnTime;

            if (timer >= spawnTime - 0.02f && timer <= spawnTime + 0.02f && timerStarted) /// !!!
            {
                spawnPipeline[iterator].SetActive(true);
                spawnPipeline[iterator].transform.position = SetRowPosition(id: spawnPipeline[iterator].GetComponent<ObjectParameters>().EN, pos: C);
                spawnPipeline[iterator].transform.rotation = Quaternion.identity;
                spawnPipeline[iterator].transform.localEulerAngles = rot;
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

    public void SetReleasePointPosition(GameObject releasePoint, Hand handSide)
    {
        ReleaseIcon rel = releasePoint.GetComponentInChildren<ReleaseIcon>();
        Vector3 finalPosition, currentPosition = releasePoint.transform.position, startPosition = C;
        int ID = releasePoint.GetComponent<ObjectParameters>().linkedCatchId;
        if (handSide == Hand.Left)
        {
            finalPosition = new Vector3(startPosition.x + 0.25f, currentPosition.y, currentPosition.z);
            switch (spawnPipeline[ID].GetComponent<ObjectParameters>().color)
            {
                case EntityColour.Green:
                    rel.LeftGreen();
                    break;
                case EntityColour.Red:
                    rel.LeftRed();
                    break;
                default:
                    rel.Rotten();
                    break;
            }
        }
        else
        {
            finalPosition = new Vector3(startPosition.x + 1.25f, currentPosition.y, currentPosition.z);
            switch (spawnPipeline[ID].GetComponent<ObjectParameters>().color)
            {
                case EntityColour.Green:
                    rel.RightGreen();
                    break;
                case EntityColour.Red:
                    rel.RightRed();
                    break;
                default:
                    rel.Rotten();
                    break;
            }
        }

        releasePoint.transform.position = finalPosition;
    }
}
