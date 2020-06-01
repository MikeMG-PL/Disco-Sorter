using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Kolider w pudle, do którego mamy wrzucać jabłka, sprawdza czy zostały spełnione wszystkie warunki i czy możemy dostać za jabłko punkty

    public EntityColour color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            ObjectParameters parameters = other.GetComponentInParent<ObjectParameters>();

            if (parameters.color == color && parameters.catchWasDoneOnTime && !parameters.wasPreviouslyInserted)
            {
                if (parameters.action == EntityAction.Slap && !parameters.wasGrabbed)
                {
                    Debug.Log("Point slap");
                }

                else if (parameters.action == EntityAction.Throw && parameters.wasGrabbed)
                {
                    Debug.Log("Point throw");
                }

                else if (parameters.action == EntityAction.CatchAndRelease && parameters.releaseWasDoneOnTime)
                {
                    Debug.Log("Point catch&release");
                }
            }

            parameters.wasPreviouslyInserted = true;
        }
    }
}
