using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameters : MonoBehaviour
{
    // EN oznacza entity number, odnosi się do tablicy wszystkich obiektów (nawet "pustych", nieprzypisanych w edytorze piosenki)
    // Id odnosi się do miejsca w tablicy (spawnPipeline) obiektów będących faktycznie w grze
    public float actionTime;
    public float actionStartTime;
    public float actionEndTime;
    public float linkedReleaseTimeStart;
    public float linkedReleaseTimeEnd;
    public float linkedCatchTime;
    public float spawnTime;

    public int linkedReleaseEN;
    public int linkedCatchEN;
    public int linkedReleaseId;
    public int linkedCatchId;
    public int EN;
    public int Id;

    public EntityType type;
    public EntityColour color;
    public EntityAction action;

    public bool wasInserted;
    public bool wasMissed;
    public bool wasGrabbed;
    public bool wasReleased;
    public bool wasCatchedOnTime;
    public bool wasReleasedOnTime;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }
}
