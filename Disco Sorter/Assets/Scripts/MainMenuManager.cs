using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static int reds = 0, greens = 0, yellows = 0;
    [HideInInspector()]
    public bool blocked;

    int id = -1;
    public List<ArrowLights> lights;
    public HandEvents left, right;
    [Header("----------------------")]
    public Transform redPos, greenPos, yellowPos;
    [Header("----------------------")]
    public GameObject red, green, yellow;

    bool performingLeft, performingRight;

    void Awake()
    {
        reds = 0; greens = 0; yellows = 0;
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
        if (!blocked)
            Count();
        if (!performingLeft)
            CheckIfGrabbed(left);
        if (!performingRight)
            CheckIfGrabbed(right);
    }

    void Coroutines(HandEvents h)
    {
        StartCoroutine(lights[id].fixedBlinkBloom((ArrowManager.Light)id, (ArrowManager.Hand)h.handSide, 1));
        StartCoroutine(lights[id].fixedBlinkColor((ArrowManager.Light)id, (ArrowManager.Hand)h.handSide, 1));
        StartCoroutine(Wait(2, h));
    }

    void CheckIfGrabbed(HandEvents h)
    {
        if (h.parameters != null)
        {
            if (h.handSide == Hand.Left)
                performingLeft = true;
            else
                performingRight = true;

            switch (h.parameters.color)
            {
                case EntityColour.Red:
                    id = 0;
                    Coroutines(h);
                    break;

                case EntityColour.Green:
                    id = 1;
                    Coroutines(h);
                    break;

                case EntityColour.None:
                    id = 2;
                    Coroutines(h);
                    break;
            }
        }
    }

    public IEnumerator Wait(float t, HandEvents h)
    {
        yield return new WaitForSeconds(t);

        if (h.handSide == Hand.Left)
            performingLeft = false;
        else
            performingRight = false;
    }

    Material r, g, y; GameObject ro, go, yo;
}
