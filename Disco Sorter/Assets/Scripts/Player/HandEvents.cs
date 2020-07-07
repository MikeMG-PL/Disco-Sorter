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

    [Header("Hands stuff")]
    public Hand handSide;
    public GameObject discoFractured;
    public bool holding;

    [HideInInspector()]
    public ObjectParameters parameters;

    private Player player;
    private PlayerActions playerActions;
    private LevelManager levelManager;
    private bool thisIsGame;

    private void Start()
    {
        if (GetComponent<VRTK_InteractGrab>() == null) Debug.Log("Error, there's no InteractGrab script in the object");

        player = GetComponentInParent<Player>();
        playerActions = GetComponentInParent<PlayerActions>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "2.GAME") thisIsGame = true;
        if (thisIsGame) levelManager = player.levelManagerObject.GetComponent<LevelManager>();
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

        if (!parameters.wasGrabbed && thisIsGame)
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

        if (!parameters.wasReleased && thisIsGame)
            playerActions.CheckActionTime(parameters, false);

        parameters.wasReleased = true;
        holding = false;
        parameters = null;
    }

    public void OnDiscoHit(ObjectParameters parameters)
    {
        if (thisIsGame)
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
