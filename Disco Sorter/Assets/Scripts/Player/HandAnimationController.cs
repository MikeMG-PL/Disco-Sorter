using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class HandAnimationController : MonoBehaviour
{
    public float handFistStage;
    public VRTK_ControllerEvents controllerEvents;
    public Animator handAnimator;

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
    }

    private void Update()
    {
        //trigger axis doesn't work somehow, need to check on Quest
        handFistStage = controllerEvents.GetTriggerAxis();
        //handFistStage = controllerEvents.GetGripAxis();
        //controllerEvents.GetGripAxis();
        //handFistStage = 0.9f;
        handAnimator.SetFloat("TriggerPressed", handFistStage);

    }
}
