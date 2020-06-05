using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public List<GameObject> lasers;
    int howMany;
    public LevelManager levelManager;

    void Start()
    {
        howMany = GameObject.FindGameObjectsWithTag("Laser").GetLength(0);
        GameObject[] tab = new GameObject[howMany];
        tab = GameObject.FindGameObjectsWithTag("Laser");

        for (int i = 0; i < howMany; i++)
        {
            lasers.Add(tab[i]);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        Transform p = other.transform.parent;

        if ((p.tag == "DiscoBall" || p.tag == "Apple" || p.tag == "RottenApple" && other.transform.parent.GetComponent<ObjectParameters>().actionStartTime - 0.2f <= levelManager.timer))
        {   
            for (int i = 0; i < howMany; i++)
            {
                lasers[i].GetComponent<Laser>().col = (Laser.LaserColor)Random.Range(0, 5);
            }
        }
    }
}
