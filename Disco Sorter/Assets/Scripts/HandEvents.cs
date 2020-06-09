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

enum ActionHighlight { Unknown, Success, Fail };

public class HandEvents : MonoBehaviour
{
    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette stuff")]
    public Material vignette;
    public float fadeSpeed = 60;
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
        OnTime();

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

        StartCoroutine(VignetteAnim());
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

    IEnumerator VignetteAnim()
    {
        float maxAlpha = 0.75f;

        while (alpha <= maxAlpha)
        {
            vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
            vignette.SetColor("_EmissionColor", vignette.color);
            alpha += fadeSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (alpha > 0)
        {
            vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
            vignette.SetColor("_EmissionColor", vignette.color);
            alpha -= fadeSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    
}
