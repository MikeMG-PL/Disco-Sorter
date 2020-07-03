using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public List<ArrowLights> lights;
    public HandEvents left, right;
    [Header("----------------------")]
    public Transform redPos, greenPos, yellowPos;
    [Header("----------------------")]
    public GameObject red, green, yellow;

    void Start()
    {
        SpawnApples();
    }

    void Update()
    {
        CheckIfGrabbed(left);
        CheckIfGrabbed(right);
    }

    void CheckIfGrabbed(HandEvents h)
    {
        if (h.parameters != null && h.holding)
        {
            switch (h.parameters.color)
            {
                case EntityColour.Red:

                    lights[0].blinkSpeed = 3;
                    lights[0].myLight = true;

                    break;

                case EntityColour.Green:

                    lights[1].blinkSpeed = 3;
                    lights[1].myLight = true;

                    break;

                case EntityColour.None:

                    lights[2].blinkSpeed = 3;
                    lights[2].myLight = true;

                    break;
            }
        }
        else if(h.parameters != null && !h.holding)
        {
            lights[0].myLight = false;
            lights[1].myLight = false;
            lights[2].myLight = false;
        }
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
