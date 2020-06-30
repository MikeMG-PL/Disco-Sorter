using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Kolider w pudle, do którego mamy wrzucać jabłka, sprawdza czy zostały spełnione wszystkie warunki i czy możemy dostać za jabłko punkty

    public EntityColour color; SFXManager sfx;
    PointManager pointManager;

    void Start()
    {
        pointManager = GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>();
        sfx = GetComponent<SFXManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            ObjectParameters parameters = other.GetComponentInParent<ObjectParameters>();

            if (parameters.type == EntityType.RottenApple && color == EntityColour.None)
            {
                parameters.wasInserted = true;
                sfx.PlaySound(sfx.correctBox);
                pointManager.ThrowPoints(PointManager.AppleState.CorrectBox, 0);
            }
            else if (parameters.color == color && /*parameters.wasCatchedOnTime &&*/ !parameters.wasInserted)
            {
                parameters.wasInserted = true;
                sfx.PlaySound(sfx.correctBox);
                pointManager.ThrowPoints(PointManager.AppleState.CorrectBox, 0);
            }
            else if (parameters.color != color && /*parameters.wasCatchedOnTime &&*/ !parameters.wasInserted)
            {
                sfx.PlaySound(sfx.customClips[0]);
                pointManager.ThrowPoints(PointManager.AppleState.IncorrectBox, 0);
            }

        }
    }
}
