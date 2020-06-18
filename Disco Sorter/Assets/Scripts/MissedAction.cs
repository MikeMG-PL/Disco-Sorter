using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedAction : MonoBehaviour
{
    public LevelManager levelManager;
    public OnScreen onScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Release"))
        {
            StartCoroutine(other.GetComponent<ReleaseIcon>().Disable());
            StartCoroutine(other.GetComponent<ReleaseIcon>().DisableFog());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;
        if (p.GetComponent<ObjectParameters>() == null) return;
        ObjectParameters o = p.GetComponent<ObjectParameters>();

        if ((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple") || p.CompareTag("Release")) && !o.wasGrabbed)
        {
            if(!o.wasMissed)
                onScreen.HighlightVignette(ActionHighlight.Fail);
            o.wasMissed = true;
        }
    }
}
