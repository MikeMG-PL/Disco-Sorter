using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FistClench : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public Animator fistAnim;
    public float fistState;

    private void Update()
    {
        fistAnim.SetFloat("DiscoHit", fistState);
    }

    void OnTriggerEnter(Collider other)
    {
        Transform p = other.transform.parent;

        if (p.CompareTag("DiscoBall"))
            fistAnim.SetBool("NearDisco", true);
    }

    void OnTriggerExit(Collider other)
    {
        Transform p = other.transform.parent;

        if (p.CompareTag("DiscoBall"))
            fistAnim.SetBool("NearDisco", false);
    }
}