using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameters : MonoBehaviour
{
    public float actionTime;
    public float actionStartTime;
    public float actionEndTime;
    public float linkedReleaseTimeStart;
    public float linkedReleaseTimeEnd;
    public float linkedCatchTime;
    public int linkedReleaseEN;
    public int linkedCatchEN;
    public float spawnTime;

    public EntityType type;
    public EntityColour color;
    public EntityAction action;
    public int ID;

    public bool wasPreviouslyInserted;
    public bool wasGrabbed;
    public bool wasReleased;
    public bool wasCatchedOnTime;
    public bool wasReleasedOnTime;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }
}
