using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public EntityColour color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            ObjectParameters parameters = other.GetComponentInParent<ObjectParameters>();
            if (parameters.color == color && !parameters.wasPreviouslyInserted)
            {
                if (parameters.action == EntityAction.Slap && !parameters.wasPickedUp)
                {
                    Debug.Log("Point");
                }

                else if (parameters.action == EntityAction.Throw && parameters.wasPickedUp)
                {
                    Debug.Log("Point");
                }
            }

            parameters.wasPreviouslyInserted = true;
        }
    }
}
