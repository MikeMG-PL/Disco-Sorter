using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public LevelManager levelManager;
    public float blinkSpeed;
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
            lights[i].blinkSpeed = blinkSpeed;
            lights[i].blinkColor = blinkColor;
        }
    }

    float distance;
    ObjectParameters o;
    float speed;
    FinalBlink[] components;
    void DistanceCheck()
    {
        if (leftHand.parameters != null)
            o = leftHand.parameters;
        if (rightHand.parameters != null)
            o = rightHand.parameters;

        if (o != null && levelManager.spawnPipeline[o.linkedReleaseId].gameObject != null && !levelManager.spawnPipeline[o.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased)
        {
            components = GetComponentsInChildren<FinalBlink>();
            distance = Vector3.Distance(levelManager.spawnPipeline[o.linkedReleaseId].transform.position, finish.position);
            blinkSpeed = 1;
            previousID = o.linkedReleaseId;

            if (LevelManager.timer >= o.linkedReleaseTimeStart + 0.02f && LevelManager.timer <= o.linkedReleaseTimeEnd - 0.02f && !set)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    switch (light)
                    {
                        case Light.Red:
                            if (components[i].GetComponent<ArrowLights>().light == Light.Red)
                                StartCoroutine(components[i].GetComponent<FinalBlink>().Enable());
                            break;

                        case Light.Green:
                            if (components[i].GetComponent<ArrowLights>().light == Light.Green)
                                StartCoroutine(components[i].GetComponent<FinalBlink>().Enable());
                            break;

                        case Light.Yellow:
                            if (components[i].GetComponent<ArrowLights>().light == Light.Yellow)
                                StartCoroutine(components[i].GetComponent<FinalBlink>().Enable());
                            break;

                        case Light.None:
                            break;
                    }

                }
                set = true;
            }
        }
        else
        {
            light = Light.None;
            set = false;
        }
    }

    void ColorCheck()
    {
        if (o != null && levelManager.spawnPipeline[o.linkedReleaseId].gameObject != null &&
            !levelManager.spawnPipeline[o.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased && !set && LevelManager.timer >= o.linkedReleaseTimeStart - 1)
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
        if (blinkSpeed <= 0)
            blinkSpeed = 1;

        Illuminate();
    }

    private void FixedUpdate()
    {
        DistanceCheck();
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
                    lights[i].NoColor();
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
            lights[i].blinkSpeed = blinkSpeed;
            lights[i].blinkColor = blinkColor;
            if (lights[i].light == light)
                lights[i].myLight = true;
            else
                lights[i].myLight = false;
        }
    }
}
