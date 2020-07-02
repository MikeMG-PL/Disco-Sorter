using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public LevelManager levelManager;
    [HideInInspector()]
    public Color blinkColor;
    ArrowLights[] lights;
    TopYellowArrow[] topYellows;
    bool loopDone, illuminate = false; bool set;
    int d, previousID;
    [HideInInspector()]
    public bool isChecked;
    public enum Light { None, Red, Green, Yellow }; public new Light light;
    ///////////////////////////////////////////////////////////////////////
    public HandEvents leftHand, rightHand;

    public Queue<GameObject> releasePipeline; public Transform finish;

    void Start()
    {
        light = Light.None;
        releasePipeline = new Queue<GameObject>();

        lights = GetComponentsInChildren<ArrowLights>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].blinkColor = blinkColor;
        }
    }

    ObjectParameters o;
    void ColorCheck()
    {
        if (leftHand.parameters != null)
            o = leftHand.parameters;
        if (rightHand.parameters != null)
            o = rightHand.parameters;

        if (o != null && levelManager.spawnPipeline[o.linkedReleaseId].gameObject != null &&
            !levelManager.spawnPipeline[o.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased
            && LevelManager.timer >= (o.linkedReleaseTimeStart + o.linkedReleaseTimeEnd)/2 - 1.11f)
        {
            if (o.type == EntityType.RottenApple)
                light = Light.Yellow;
            if (o.type == EntityType.Apple)
            {
                switch (o.color)
                {
                    case EntityColour.Green:
                        light = Light.Green;
                        break;

                    case EntityColour.Red:
                        light = Light.Red;
                        break;

                    default:
                        break;
                }
            }
        }
        else
            light = Light.None;
    }

    void Update()
    {
        Illuminate();
    }

    private void FixedUpdate()
    {
        ColorCheck();
    }

    void Illuminate()
    {
        topYellows = GetComponentsInChildren<TopYellowArrow>();
        switch (light)
        {
            case Light.None:

                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].myLight = false;
                }
                break;

            case Light.Red:
                for (int i = 0; i < topYellows.Length; i++)
                {
                    SetYellow(false, i);
                }
                Set(Color.red);
                break;

            case Light.Green:
                for (int i = 0; i < topYellows.Length; i++)
                {
                    SetYellow(false, i);
                }
                Set(Color.green);
                break;

            case Light.Yellow:

                for (int i = 0; i < topYellows.Length; i++)
                {
                    SetYellow(true, i);
                }
                Set(Color.yellow);
                break;
        }

    }



    void SetYellow(bool isYellow, int iterator)
    {
        if (topYellows[iterator].gameObject.GetComponent<ArrowLights>().topYellow)
        {
            topYellows[iterator].transform.parent.GetComponent<MeshRenderer>().enabled = !isYellow;
            topYellows[iterator].GetComponent<MeshRenderer>().enabled = !isYellow;
        }
        else
        {
            topYellows[iterator].transform.parent.GetComponent<MeshRenderer>().enabled = isYellow;
            topYellows[iterator].GetComponent<MeshRenderer>().enabled = isYellow;
        }
    }

    void Set(Color c)
    {
        blinkColor = c;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].blinkColor = blinkColor;
            if (lights[i].light == light)
                lights[i].myLight = true;
            else
                lights[i].myLight = false;
        }
    }
}
