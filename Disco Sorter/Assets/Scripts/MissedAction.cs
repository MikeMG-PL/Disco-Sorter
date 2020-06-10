using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedAction : MonoBehaviour
{
    public OnScreen onScreen;

    private void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;
        ObjectParameters o = p.GetComponent<ObjectParameters>();

        if((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple")) && !o.wasGrabbed)
        {
            onScreen.HighlightVignette(ActionHighlight.Fail);
        }
        else if((p.CompareTag("Release")) && !o.wasReleased)
        {
            onScreen.HighlightVignette(ActionHighlight.Fail);
        }
    }
}
