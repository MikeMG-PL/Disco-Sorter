using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRendererDisable : MonoBehaviour
{
    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (!renderers[i].gameObject.CompareTag("Gloves"))
                renderers[i].enabled = false;
        }
    }
}
