using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedAction : MonoBehaviour
{
    public LevelManager levelManager;
    public OnScreen onScreen;

    private void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;

        if (p.GetComponent<ObjectParameters>() == null) return;

        ObjectParameters o = p.GetComponent<ObjectParameters>();

        if ((p.CompareTag("DiscoBall") || p.CompareTag("Apple") || p.CompareTag("RottenApple")) && !o.wasGrabbed)
        {
            onScreen.HighlightVignette(ActionHighlight.Fail);
        }
    }
}
