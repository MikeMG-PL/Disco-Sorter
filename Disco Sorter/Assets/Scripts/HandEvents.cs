using OculusSampleFramework;
using OVR.OpenVR;
using UnityEngine;
using VRTK;

public enum Hand
{
    Right,
    Left,
};

enum ActionHighlight { Unknown, Success, Fail };

public class HandEvents : MonoBehaviour
{
    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette stuff")]
    public Material vignette;
    public float fadeSpeed = 60;
    float tempTimer = 100;
    float alpha;

    Color g, r;

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
        g = new Color(0, 1, 0, alpha);
        r = new Color(1, 0, 0, alpha);
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
            HighlightVignette(ActionHighlight.Success);
        }
        else
        {
            HighlightVignette(ActionHighlight.Fail);
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
                HighlightVignette(ActionHighlight.Success);
            }
            else
            {
                HighlightVignette(ActionHighlight.Fail);
            }
        }
    }

    void HighlightVignette(ActionHighlight h)
    {
        switch (h)
        {
            case ActionHighlight.Success:
                vignette.color = g;
                break;
            case ActionHighlight.Fail:
                vignette.color = r;
                break;
            default:
                vignette.color = new Color(0, 0, 0, 0);
                break;
        }

        if ((parameters.wasCatchedOnTime == false && parameters.action != EntityAction.ReleasePoint)
            || (parameters.wasReleasedOnTime == false && parameters.action == EntityAction.ReleasePoint))
        {
            tempTimer = 0;
        }
        if (tempTimer > 0.5f)
            OnTime();

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

    void VignetteAnim()
    {
        float maxAlpha = 0.5f, minAlpha = 0, step0 = 0, step1 = 0.2f, step2 = 0.5f;
        tempTimer += Time.deltaTime;
        if (tempTimer > step0 && alpha <= maxAlpha && tempTimer <= step1)
        {
            alpha += fadeSpeed * Time.deltaTime;
        }
        else if (tempTimer > step1 && tempTimer <= step2)
        {
            alpha -= fadeSpeed * Time.deltaTime * 2;
        }
        else if (tempTimer > step2)
        {
            alpha = minAlpha;
        }
    }

    void VignetteFlash()
    {
        vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
        vignette.SetColor("_EmissionColor", vignette.color);
        VignetteAnim();
    }

    void Update()
    {
        VignetteFlash();
    }

}
