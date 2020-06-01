using UnityEngine;
using VRTK;

public class HandsEvents : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += OnGrabObject;
    }

    private void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() != null)
             e.target.GetComponent<ObjectParameters>().wasGrabbed = true;
    }
}
