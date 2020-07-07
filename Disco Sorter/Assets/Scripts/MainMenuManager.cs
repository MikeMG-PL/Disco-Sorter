using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static int reds = 0, greens = 0, yellows = 0;

    public List<ArrowLights> lights;
    public HandEvents left, right;
    [Header("----------------------")]
    public Transform redPos, greenPos, yellowPos;
    [Header("----------------------")]
    public GameObject red, green, yellow;

    void Start()
    {
        //SpawnApples();
    }

    void Count()
    {
        if (reds < 1)
        {
            StartCoroutine(COUNTRED());
            reds++;
        }
        if (greens < 1)
        {
            StartCoroutine(COUNTGREEN());
            greens++;
        }
        if (yellows < 1)
        {
            StartCoroutine(COUNTYELLOW());
            yellows++;
        }


    }

    public IEnumerator COUNTRED()
    {
        yield return new WaitForSeconds(2.5f);
        r = red.GetComponent<ObjectMethods>().dissolveMaterial;
        ro = Instantiate(red, redPos.position, Quaternion.identity);
        ro.GetComponent<ObjectParameters>().color = EntityColour.Red;
        red.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = r;
        StartCoroutine(red.GetComponent<ObjectMethods>().RevertedDissolve());
        r.SetFloat("_DissolveAmount", 1);
        StopCoroutine(COUNTRED());
    }

    public IEnumerator COUNTGREEN()
    {
        yield return new WaitForSeconds(2.5f);
        g = green.GetComponent<ObjectMethods>().dissolveMaterial;
        go = Instantiate(green, greenPos.position, Quaternion.identity);
        go.GetComponent<ObjectParameters>().color = EntityColour.Green;
        green.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = g;
        StartCoroutine(green.GetComponent<ObjectMethods>().RevertedDissolve());
        g.SetFloat("_DissolveAmount", 1);
        StopCoroutine(COUNTGREEN());
    }

    public IEnumerator COUNTYELLOW()
    {
        yield return new WaitForSeconds(2.5f);
        y = yellow.GetComponent<ObjectMethods>().dissolveMaterial;
        yo = Instantiate(yellow, yellowPos.position, Quaternion.identity);
        yo.GetComponent<ObjectParameters>().color = EntityColour.None;
        yellow.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = y;
        StartCoroutine(yellow.GetComponent<ObjectMethods>().RevertedDissolve());
        y.SetFloat("_DissolveAmount", 1);
        StopCoroutine(COUNTYELLOW());
    }

    void Update()
    {
        Debug.Log(greens);
        Count();
        CheckIfGrabbed(left);
        CheckIfGrabbed(right);
    }

    void CheckIfGrabbed(HandEvents h)
    {

    }

    Material r, g, y; GameObject ro, go, yo;
    void SpawnApples()
    {
        r = red.GetComponent<ObjectMethods>().dissolveMaterial;
        g = green.GetComponent<ObjectMethods>().dissolveMaterial;
        y = yellow.GetComponent<ObjectMethods>().dissolveMaterial;

        ro = Instantiate(red, redPos.position, Quaternion.identity);
        go = Instantiate(green, greenPos.position, Quaternion.identity);
        yo = Instantiate(yellow, yellowPos.position, Quaternion.identity);

        ro.GetComponent<ObjectParameters>().color = EntityColour.Red;
        go.GetComponent<ObjectParameters>().color = EntityColour.Green;
        yo.GetComponent<ObjectParameters>().color = EntityColour.None;

        red.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = r;
        green.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = g;
        yellow.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = y;

        r.SetFloat("_DissolveAmount", 1);
        g.SetFloat("_DissolveAmount", 1);
        y.SetFloat("_DissolveAmount", 1);

        StartCoroutine(red.GetComponent<ObjectMethods>().RevertedDissolve());
        StartCoroutine(green.GetComponent<ObjectMethods>().RevertedDissolve());
        StartCoroutine(yellow.GetComponent<ObjectMethods>().RevertedDissolve());
    }
}
