using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteManager : MonoBehaviour
{
    public Material vignette; Color r, g; float alpha; bool highlighted; public float vignetteFadeSpeed = 10;

    void Start()
    {
        vignette.color = new Color(0, 0, 0, 0);
        g = new Color(0, 1, 0, alpha);
        r = new Color(1, 0, 0, alpha);
    }

    public void HighlightVignette(ActionHighlight h)
    {
        switch (h)
        {
            case ActionHighlight.Success:
                vignette.color = g;
                break;
            case ActionHighlight.Fail:
                vignette.color = r;
                break;
            default:
                vignette.color = new Color(0, 0, 0, 0);
                break;
        }

        StartCoroutine(VignetteAnim());
    }

    void OnTime(ObjectParameters parameters)
    {
        switch (parameters.action)
        {
            case EntityAction.ReleasePoint:
                parameters.wasReleasedOnTime = true;
                break;
            default:
                parameters.wasCatchedOnTime = true;
                break;
        }
    }

    public IEnumerator VignetteAnim()
    {
        if (!highlighted)
        {
            highlighted = true;
            float maxAlpha = 0.75f;

            while (alpha <= maxAlpha)
            {
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                vignette.SetColor("_EmissionColor", vignette.color);
                alpha += vignetteFadeSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            while (alpha > 0)
            {
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                vignette.SetColor("_EmissionColor", vignette.color);
                alpha -= vignetteFadeSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            highlighted = false;
        }

    }
}
