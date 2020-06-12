using OculusSampleFramework;
using OVR.OpenVR;
using System.Collections;
using UnityEngine;
using VRTK;

public enum Hand { Right, Left };
public enum ActionHighlight { Unknown, Success, Fail };

public class HandEvents : MonoBehaviour
{
    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette")]
    public OnScreen onScreen;

    // Eventy dotyczące tego, co robią ręce gracza, np. złapanie obiektu, wyrzucenie obiektu. Są doczepione do Left i Right ControllerScriptAlias

    [Header("Hands stuff")]
    public Hand handSide;
    private Player player;
    private LevelManager levelManager;
    ObjectParameters parameters;

    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        player = GetComponentInParent<Player>();
        levelManager = player.levelManagerObject.GetComponent<LevelManager>();

        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject += OnUngrabObject;
    }

    private void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;
        parameters = e.target.GetComponent<ObjectParameters>();
        parameters.wasGrabbed = true;
        if (handSide == Hand.Right) player.rightHandGrabbedObject = e.target;
        else player.leftHandGrabbedObject = e.target;

        CheckActionTime(parameters, true);
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;
        parameters = e.target.GetComponent<ObjectParameters>();
        parameters.wasReleased = true;
        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        CheckActionTime(parameters, false);
    }

    void CheckActionTime(ObjectParameters parameters, bool thisIsGrabbing)
    {
        float timer = levelManager.timer;
        float clamp1 = 0, clamp2 = 0;

        switch (thisIsGrabbing)
        {
            case true:
                clamp1 = parameters.actionStartTime; clamp2 = parameters.actionEndTime;
                break;

            case false:
                clamp1 = parameters.linkedReleaseTimeStart; clamp2 = parameters.linkedReleaseTimeEnd;
                break;
        }

        if (parameters.action == EntityAction.CatchAndRelease)
        {
            if (timer >= clamp1 && timer <= clamp2)
            {
                onScreen.HighlightVignette(ActionHighlight.Success);
                OnTime(parameters);
            }
            else
                onScreen.HighlightVignette(ActionHighlight.Fail);
        }
    }

    void OnTime(ObjectParameters parameters)
    {
        switch (parameters.action)
        {
            case EntityAction.CatchAndRelease:
                parameters.wasReleasedOnTime = true;
                break;
            default:
                parameters.wasCatchedOnTime = true;
                break;
        }
    }
}
