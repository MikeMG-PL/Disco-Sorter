using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public SongController songController;
    public LevelParameters level;

    Vector3 A, B, C;
    public Transform TransformOfB;
    public GameObject testBallPrefab;

    void Calculations()
    {
        var b = (Mathf.Pow(level.rollTime, 2) * Mathf.Abs(Physics.gravity.y) * Mathf.Sin(0.174532925f)) / (2 * (Mathf.Pow(Mathf.Sin(0.174532925f), 2) + 1) * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2)));
        var h = (b * Mathf.Sin(0.174532925f)) / Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(0.174532925f), 2));

        B = TransformOfB.position;
        A = new Vector3 { x = B.x, y = B.y, z = B.z + b };
        C = new Vector3 { x = A.x, y = A.y + h, z = A.z };

        Instantiate(testBallPrefab, C, Quaternion.identity);
    }

    void PlayMusic()
    {

    }

    void SpawnObjects()
    {

    }

    void RythmCheck()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            Calculations();
    }
}
