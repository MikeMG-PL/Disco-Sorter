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
        GameObject[] tab;
        tab = GameObject.FindGameObjectsWithTag("Laser");

        for (int i = 0; i < howMany; i++)
        {
            lasers.Add(tab[i]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform p = other.transform.parent;

        if ((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple") && other.transform.parent.GetComponent<ObjectParameters>().actionStartTime - 0.2f <= levelManager.timer))
        {
            for (int i = 0; i < howMany; i++)
            {
                Laser l = lasers[i].GetComponent<Laser>();
                l.col = (Laser.LaserColor)Random.Range(0, l.colors.Count);

                // Losowanie z wyłącznością poprzedniego koloru
                if (l.col == 0 && l.col == l.prev && l.colors.Count != 1)
                    l.col++;
                if (l.col == l.prev && l.col > 0 && l.colors.Count != 1)
                    l.col--;
                l.prev = l.col;
            }
        }
    }
}
