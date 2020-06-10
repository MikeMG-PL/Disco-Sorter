using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedAction : MonoBehaviour
{
    [Tooltip("Does not matter whether left or right hand script is attached")]
    public HandEvents handEvents;

    void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;
        ObjectParameters o = other.transform.parent.GetComponent<ObjectParameters>();

        if ((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple")) && !o.wasGrabbed)
        {
            handEvents.HighlightVignette(ActionHighlight.Fail);
        }
        if (p.CompareTag("Release") && !o.wasReleased)
        {
            handEvents.HighlightVignette(ActionHighlight.Fail);
        }
    }
}
