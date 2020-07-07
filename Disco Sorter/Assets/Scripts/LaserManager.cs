using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public List<GameObject> lasers;
    int howMany;
    public LevelManager levelManager;
    bool illuminate;


    void Start()
    {
        howMany = GameObject.FindGameObjectsWithTag("Laser").GetLength(0);
        GameObject[] tab;
        tab = GameObject.FindGameObjectsWithTag("Laser");

        for (int i = 0; i < howMany; i++)
        {
            lasers.Add(tab[i]);
        }
        StartCoroutine(Illuminations());
    }

    void OnTriggerEnter(Collider other)
    {
        Transform p = other.transform.parent;

        if ((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple") || p.CompareTag("Release"))
            && other.transform.parent.GetComponent<ObjectParameters>().actionEndTime + 0.2f >= LevelManager.timer)
            illuminate = true;
    }

    IEnumerator Illuminations()
    {
        while (true)
        {
            if (illuminate)
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

                    if(l.timer < 0.5f)
                        l.timer = 0;

                    if (l.direction == Laser.Dir.right)
                        l.direction = Laser.Dir.left;
                    else
                        l.direction = Laser.Dir.right;
                }
            }
            illuminate = false;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
