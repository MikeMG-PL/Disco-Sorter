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
    // public float linkedCatchTime;                  // Opcjonalnie, jeśli akcja to Catch... release (release)
    public float spawnTime;

    public EntityType type;
    public EntityColour color;
    public EntityAction action;
    public int ID;

    public bool wasPreviouslyInserted;
    public bool wasGrabbed;
    public bool catchWasDoneOnTime;
    public bool releaseWasDoneOnTime;

    void Update()
    {
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }
}
