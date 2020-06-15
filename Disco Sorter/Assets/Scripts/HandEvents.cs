using OculusSampleFramework;
using OVR.OpenVR;
using System.Collections;
using UnityEngine;
using VRTK;

public enum Hand { Right, Left };
public enum ActionHighlight { Unknown, Success, Fail };

public class HandEvents : MonoBehaviour
{
    // Skrypt dotyczący tego, co robi gracz rękami, np. złapanie obiektu, wyrzucenie obiektu. Są doczepione do Left i Right ControllerScriptAlias

    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette")]
    public OnScreen onScreen;

    [Header("Hands stuff")]
    public Hand handSide;
    public GameObject discoFractured;

    private Player player;
    private LevelManager levelManager;
    private ObjectParameters parameters;


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
        parameters = e.target.GetComponent<ObjectParameters>();

        if (handSide == Hand.Right) player.rightHandGrabbedObject = e.target;
        else player.leftHandGrabbedObject = e.target;

        if (!parameters.wasGrabbed)
        {
            CheckActionTime(parameters, true);

            if (parameters.action == EntityAction.CatchAndRelease)
                levelManager.SetReleasePointPosition(levelManager.spawnPipeline[parameters.linkedReleaseId], handSide);
        }

        parameters.wasGrabbed = true;
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;
        parameters = e.target.GetComponent<ObjectParameters>();

        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        if (!parameters.wasReleased)
            CheckActionTime(parameters, false);

        parameters.wasReleased = true;
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
        float actionStart, actionEnd;

        switch (thisIsGrabbingOrDisco)
        {
            case true:
                actionStart = parameters.actionStartTime; actionEnd = parameters.actionEndTime;

                if (timer >= actionStart && timer <= actionEnd)
                {
                    onScreen.HighlightVignette(ActionHighlight.Success);
                    parameters.wasCatchedOnTime = true;
                }

                else
                    onScreen.HighlightVignette(ActionHighlight.Fail);
                break;

            case false:
                if (parameters.action != EntityAction.CatchAndRelease) return;
                actionStart = parameters.linkedReleaseTimeStart; actionEnd = parameters.linkedReleaseTimeEnd;

                if (timer >= actionStart && timer <= actionEnd)
                {
                    onScreen.HighlightVignette(ActionHighlight.Success);
                    parameters.wasReleasedOnTime = true;
                }

                else
                {
                    onScreen.HighlightVignette(ActionHighlight.Fail);
                    levelManager.spawnPipeline[parameters.linkedReleaseId].GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
                break;
        }
    }

    private void OnDisable()
    {
        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject -= OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject -= OnUngrabObject;
    }
}
