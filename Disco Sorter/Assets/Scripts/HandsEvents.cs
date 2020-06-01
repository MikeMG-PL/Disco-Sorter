using UnityEngine;
using VRTK;

public class HandsEvents : MonoBehaviour
{
    // Eventy dotyczące tego, co robią ręce gracza, np. złapanie obiektu, wyrzucenie obiektu. Są doczepione do Left i Right ControllerScriptAlias

    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject += OnUngrabObject;
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        ObjectParameters parameters = e.target.GetComponent<ObjectParameters>();

        if (parameters.action == EntityAction.CatchAndRelease)
        {
            float timer = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().timer;
            if (timer >= parameters.linkedReleaseTimeStart && timer <= parameters.linkedReleaseTimeEnd)
                parameters.releaseWasDoneOnTime = true;
        }
    }

    private void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        ObjectParameters parameters = e.target.GetComponent<ObjectParameters>();

        float timer = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().timer;

        if (timer >= parameters.actionStartTime && timer <= parameters.actionEndTime)
            parameters.catchWasDoneOnTime = true;

        parameters.wasGrabbed = true;
    }
}
