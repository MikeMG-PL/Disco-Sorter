using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class HandAnimationController : MonoBehaviour
{
    public float handFistStage;
    public VRTK_ControllerEvents controllerEvents;
    public VRTK_InteractGrab interactGrab;
    public Animator handAnimator;

    public float grabAppleValue = 0.4f;

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        interactGrab = GetComponent<VRTK_InteractGrab>();
    }

    private void Update()
    {

        handFistStage = controllerEvents.GetTriggerAxis();
        if (interactGrab.GetGrabbedObject() != null)
        {
            if (interactGrab.GetGrabbedObject().tag == "Apple" || interactGrab.GetGrabbedObject().tag == "RottenApple")
                handAnimator.SetFloat("TriggerPressed", grabAppleValue);
        }

        else
            handAnimator.SetFloat("TriggerPressed", handFistStage);
        //trigger axis doesn't work somehow, need to check on Quest
        //handFistStage = controllerEvents.GetGripAxis();
        //controllerEvents.GetGripAxis();
        //handFistStage = 0.9f;

    }
}
