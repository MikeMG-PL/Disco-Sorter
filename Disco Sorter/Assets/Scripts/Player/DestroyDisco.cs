using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DestroyDisco : MonoBehaviour
{
    public FistClench fistClench;
    public HandEvents handEvents;
    VRTK_VelocityEstimator v;
    public float hitSensitivity = 1.75f;

    private void Start()
    {
        v = GetComponent<VRTK_VelocityEstimator>();
        v.StartEstimation();
    }

    void OnTriggerEnter(Collider other)
    {
        Transform p = other.transform.parent;
        ObjectParameters o = p.GetComponent<ObjectParameters>();

        if (p.CompareTag("DiscoBall") && v.GetVelocityEstimate().magnitude * 1000000 > hitSensitivity)
        {
            handEvents.OnDiscoHit(o);
            fistClench.fistAnim.SetBool("NearDisco", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;
        ObjectParameters o = p.GetComponent<ObjectParameters>();

        if (p.CompareTag("DiscoBall"))
            fistClench.fistAnim.SetBool("NearDisco", false);
    }
}
