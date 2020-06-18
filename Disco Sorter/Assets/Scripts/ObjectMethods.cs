﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMethods : MonoBehaviour
{
    public GameObject discoFractured;
    public Material dissolveMaterial;
    public bool dissolve;
    bool dissolving, isChecked;
    GameObject g;
    Transform distanceCounter;

    PointManager pointManager;

    void Start()
    {
        pointManager = GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>();
        dissolveMaterial.SetFloat("_DissolveAmount", 0);
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }

    void OnCollisionEnter(Collision collision)
    {
        g = collision.gameObject;

        // punktowanie niewrzucenia jabłka do skrzyni
        if ((g.CompareTag("Plane") || g.CompareTag("Building")) && !gameObject.CompareTag("Release") &&
            !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer &&
            !isChecked && GetComponent<ObjectParameters>().wasGrabbed)
        {
            isChecked = true;

            if (gameObject.CompareTag("RottenApple"))
            {
                distanceCounter = GameObject.FindGameObjectWithTag("DistanceCounter").transform;
                pointManager.ThrowPoints(PointManager.AppleState.RottenThrow, Vector3.Distance(distanceCounter.transform.position, transform.position));
            }

            else
                pointManager.ThrowPoints(PointManager.AppleState.NoBox, 0);
        }



        if (g.CompareTag("Building") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && dissolve)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());
        }
        if (g.CompareTag("Building") && gameObject.CompareTag("DiscoBall"))
        {
            DestroyDisco();
        }
        if (g.CompareTag("Plane") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());
        }
    }

    public void DestroyDisco()
    {
        GameObject f = Instantiate(discoFractured, transform.position, Quaternion.identity);
        f.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        // punktowanie niewrzucenia jabłka do skrzyni
        if ((g.CompareTag("Plane") || g.CompareTag("Building")) && !gameObject.CompareTag("Release") &&
            !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer
            && !isChecked && GetComponent<ObjectParameters>().wasGrabbed)
        {
            isChecked = true;
            
            if(gameObject.CompareTag("RottenApple"))
            {
                distanceCounter = GameObject.FindGameObjectWithTag("DistanceCounter").transform;
                pointManager.ThrowPoints(PointManager.AppleState.RottenThrow, Vector3.Distance(distanceCounter.transform.position, transform.position));
            }
                
            else
                pointManager.ThrowPoints(PointManager.AppleState.NoBox, 0);
        }



        if (g.CompareTag("Plane") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());
        }
    }

    public IEnumerator Dissolve()
    {
        if (!dissolving)
        {
            float x = 0;
            dissolving = true;
            while (true)
            {
                dissolveMaterial.SetFloat("_DissolveAmount", Mathf.Sin(x) * 2);

                if (dissolveMaterial.GetFloat("_DissolveAmount") >= 0.86f && !GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>().levelFailed)
                {
                    Destroy(gameObject);
                    if (transform.parent != null && (transform.parent.CompareTag("Apple") || transform.CompareTag("RottenApple")))
                    {
                        Destroy(transform.parent.gameObject);
                        dissolving = false;
                    }
                }

                x += Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

    }
}
