using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Player : MonoBehaviour
{
    public GameObject levelManagerObject;

    [HideInInspector]
    public GameObject leftHandGrabbedObject, rightHandGrabbedObject;

    private void Start()
    {
        if (GetComponent<VRTK_HeadsetFade>() != null)
        {
            var headset = GetComponent<VRTK_HeadsetFade>();
            headset.Fade(Color.black, 2);
        }
    }
}
