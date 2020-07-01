using OculusSampleFramework;
using OVR.OpenVR;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using VRTK;

public enum Hand { Right, Left };
public enum ActionHighlight { Unknown, Success, Fail };

public class HandEvents : MonoBehaviour
{
    // Skrypt dotyczący tego, co robi gracz rękami, np. złapanie obiektu, wyrzucenie obiektu. Są doczepione do Left i Right ControllerScriptAlias

    [Header("Hands stuff")]
    public Hand handSide;
    public GameObject discoFractured;

    private Player player;
    private PlayerActions playerActions;
    private LevelManager levelManager;
    [HideInInspector()]
    public ObjectParameters parameters; public bool holding;


    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        player = GetComponentInParent<Player>();
        playerActions = GetComponentInParent<PlayerActions>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "3.MENU")
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

        if (!parameters.wasGrabbed && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "3.MENU")
        {
            playerActions.CheckActionTime(parameters, true);

            if (parameters.action == EntityAction.CatchAndRelease)
                levelManager.SetReleasePointPosition(levelManager.spawnPipeline[parameters.linkedReleaseId], handSide);
        }

        parameters.wasGrabbed = true;
        holding = true;
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<ObjectParameters>() == null) return;
        parameters = e.target.GetComponent<ObjectParameters>();

        if (handSide == Hand.Right) player.rightHandGrabbedObject = null;
        else player.leftHandGrabbedObject = null;

        if (!parameters.wasReleased && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "3.MENU")
            playerActions.CheckActionTime(parameters, false);

        parameters.wasReleased = true;
        holding = false;
    }

    public void OnDiscoHit(ObjectParameters parameters)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "3.MENU")
            playerActions.CheckActionTime(parameters, true);
        GameObject f = Instantiate(discoFractured, transform.position, Quaternion.identity);
        f.GetComponent<AudioSource>().Play();
        Destroy(parameters.gameObject);
    }

    private void OnDisable()
    {
        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject -= OnGrabObject;
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject -= OnUngrabObject;
    }
}
