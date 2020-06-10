using OculusSampleFramework;
using OVR.OpenVR;
using System.Collections;
using UnityEngine;
using VRTK;

public enum Hand
{
    Right,
    Left,
};

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

        if (handSide == Hand.Right) player.rightHandGrabbedObject = e.target;
        else player.leftHandGrabbedObject = e.target;

        parameters = e.target.GetComponent<ObjectParameters>();

        float timer = levelManager.timer;

        if (timer >= parameters.actionStartTime && timer <= parameters.actionEndTime)
        {
            onScreen.HighlightVignette(ActionHighlight.Success);
        }
        else
        {
            onScreen.HighlightVignette(ActionHighlight.Fail);
        }
        parameters.wasGrabbed = true;
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        parameters = e.target.GetComponent<ObjectParameters>();

        if (parameters.action == EntityAction.CatchAndRelease)
        {
            float timer = levelManager.timer;
            if (timer >= parameters.linkedReleaseTimeStart && timer <= parameters.linkedReleaseTimeEnd)
            {
                onScreen.HighlightVignette(ActionHighlight.Success);
                OnTime(parameters);
            }
            else
            {
                onScreen.HighlightVignette(ActionHighlight.Fail);
            }
        }
        parameters.wasReleased = true;
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
}
