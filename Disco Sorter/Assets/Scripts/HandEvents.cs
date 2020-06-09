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
    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette stuff")]
    public Material vignette;
    public float fadeSpeed = 60;
    float tempTimer;
    float alpha;
    bool highlight;

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

        vignette.color = new Color(0, 0, 0, 0);
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
            highlight = true;
            Debug.Log("| START: " + parameters.actionStartTime + " | HIT: " + timer + " | END:" + parameters.actionEndTime);
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
                highlight = true;
            }
        }
    }

    void HighlightVignette()
    {
        if (highlight == true)
        {
            if ((parameters.wasCatchedOnTime == false && parameters.action != EntityAction.ReleasePoint) || (parameters.wasReleasedOnTime == false && parameters.action == EntityAction.ReleasePoint))
            {
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                tempTimer += Time.deltaTime;
                if (tempTimer > 0 && alpha <= 0.5f && tempTimer <= 0.2f)
                {
                    alpha += fadeSpeed * 1.5f * Time.deltaTime;
                }
                else if (tempTimer > 0.2f && tempTimer <= 0.5f)
                {
                    alpha -= fadeSpeed * Time.deltaTime * 3.5f;
                }
                else if (tempTimer > 0.5f)
                {
                    OnTime();
                    alpha = 0;
                    highlight = false;
                    tempTimer = 0;
                }
            }
        }
    }

    void OnTime()
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

    void VignetteFlash()
    {

    }

    void Update()
    {
        HighlightVignette();
    }
}
