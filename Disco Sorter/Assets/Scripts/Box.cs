using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Kolider w pudle, do którego mamy wrzucać jabłka, sprawdza czy zostały spełnione wszystkie warunki i czy możemy dostać za jabłko punkty

    public EntityColour color; SFXManager sfx;

    void Start()
    {
        sfx = GetComponent<SFXManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            ObjectParameters parameters = other.GetComponentInParent<ObjectParameters>();

            if (parameters.color == color && /*parameters.wasCatchedOnTime &&*/ !parameters.wasInserted)
            {
                parameters.wasInserted = true;
                Debug.Log("Point");
                sfx.PlaySound(sfx.correctBox);
            }
            else if (parameters.color != color && /*parameters.wasCatchedOnTime &&*/ !parameters.wasInserted)
                sfx.PlaySound(sfx.customClips[0]);
        }
    }
}
