using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopYellowArrow : MonoBehaviour
{
    void Awake()
    {
        if (!GetComponent<ArrowLights>().topYellow)
        {
            transform.parent.gameObject.GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
