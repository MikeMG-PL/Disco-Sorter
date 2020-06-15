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
    public GameObject discoFractured;

    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        player = GetComponentInParent<Player>();
        levelManager = player.levelManagerObject.GetComponent<LevelManager>();
    }

    private void OnEnable()
    {
        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject += OnUngrabObject;
    }

    private void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        if (handSide == Hand.Right) player.rightHandGrabbedObject = e.target;
        else player.leftHandGrabbedObject = e.target;

        parameters = e.target.GetComponent<ObjectParameters>();
        parameters.wasGrabbed = true;

        if (parameters.action == EntityAction.CatchAndRelease)
        {
            levelManager.SetReleasePointPosition(levelManager.spawnPipeline[parameters.linkedReleaseId], handSide);
            levelManager.spawnPipeline[parameters.linkedReleaseId].GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        CheckActionTime(parameters, true);
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;

        //if (parameters.action == EntityAction.CatchAndRelease)
        //levelManager.SetReleasePointPosition(levelManager.spawnPipeline[parameters.linkedReleaseId]);

        parameters = e.target.GetComponent<ObjectParameters>();
        parameters.wasReleased = true;

        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        CheckActionTime(parameters, false);
    }

    public void OnDiscoHit(ObjectParameters parameters)
    {
        CheckActionTime(parameters, true);
        GameObject f = Instantiate(discoFractured, transform.position, Quaternion.identity);
        f.GetComponent<AudioSource>().Play();
        Destroy(parameters.gameObject);
    }

    void CheckActionTime(ObjectParameters parameters, bool thisIsGrabbingOrDisco)
    {
        float timer = LevelManager.timer;
        float actionStart = 0, actionEnd = 0;

        switch (thisIsGrabbingOrDisco)
        {
            case true:
                actionStart = parameters.actionStartTime; actionEnd = parameters.actionEndTime;
                break;

            case false:
                actionStart = parameters.linkedReleaseTimeStart; actionEnd = parameters.linkedReleaseTimeEnd;
                break;
        }

        if (parameters.action == EntityAction.CatchAndRelease || parameters.type == EntityType.Disco)
        {
            if (timer >= actionStart && timer <= actionEnd)
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

    private void OnDisable()
    {
        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject -= OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject -= OnUngrabObject;
    }
}
