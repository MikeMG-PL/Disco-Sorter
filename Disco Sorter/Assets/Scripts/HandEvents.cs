using OculusSampleFramework;
using OVR.OpenVR;
using UnityEngine;
using VRTK;

public enum Hand
{
    Right,
    Left,
};

public class HandEvents : MonoBehaviour
{
    // Eventy dotyczące tego, co robią ręce gracza, np. złapanie obiektu, wyrzucenie obiektu. Są doczepione do Left i Right ControllerScriptAlias

    public Hand handSide;
    private Player player;
    private LevelManager levelManager;

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

        ObjectParameters parameters = e.target.GetComponent<ObjectParameters>();

        float timer = levelManager.timer;

        if (timer >= parameters.actionStartTime && timer <= parameters.actionEndTime)
            parameters.wasCatchedOnTime = true;

        parameters.wasGrabbed = true;
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        ObjectParameters parameters = e.target.GetComponent<ObjectParameters>();

        if (parameters.action == EntityAction.CatchAndRelease)
        {
            float timer = levelManager.timer;
            if (timer >= parameters.linkedReleaseTimeStart && timer <= parameters.linkedReleaseTimeEnd)
                parameters.wasReleasedOnTime = true;
        }
    }
}
