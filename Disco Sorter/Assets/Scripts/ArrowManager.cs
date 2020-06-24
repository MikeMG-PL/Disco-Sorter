using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public float blinkSpeed;
    [HideInInspector()]
    public Color blinkColor;
    ArrowLights[] lights;
    TopYellowArrow[] topYellows;
    public enum Light { None, Red, Green, Yellow }; public new Light light;

    void Start()
    {
        lights = GetComponentsInChildren<ArrowLights>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].blinkSpeed = blinkSpeed;
            lights[i].blinkColor = blinkColor;
        }
    }

    void Update()
    {
        if (blinkSpeed <= 0)
            blinkSpeed = 1;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            light = (Light)Random.Range(1, 4);
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
                    
                    for(int i = 0; i < topYellows.Length; i++)
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
}
