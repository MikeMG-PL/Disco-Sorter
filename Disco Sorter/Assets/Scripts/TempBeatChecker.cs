using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBeatChecker : MonoBehaviour
{
    // currentObjInCollider - Lista obiektów znajdujących się w colliderze. Symuluje obiekty, które akurat mamy "pod ręką", to znaczy maksymalnie jeden rząd jabłek,
    // z kolei wciśnięcie przycisków 1-4 symuluje uderzenie tychże obiektów, w odpowiadających kolumnach.

    private List<GameObject> currentObjInCollider = new List<GameObject>();
    private int entitiesInColumn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            currentObjInCollider.Add(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ObjectParameters>() != null)
        {
            currentObjInCollider.Remove(other.transform.parent.gameObject);
        }
    }

    void Update()
    {
        Check();
    }

    // Sprawdza, który przycisk został wciśnięty
    private void Check()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CheckIfToBeat(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CheckIfToBeat(2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            CheckIfToBeat(3);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            CheckIfToBeat(4);
    }

    // Funckja sprawdzająca czy obiekt został uderzony do beatu / do akcji. Czas, w którym powinniśmy to zrobić jest określony w object parameters każdego obiektu.
    private void CheckIfToBeat(int column)
    {
        entitiesInColumn = GetComponentInParent<LevelParameters>().entitiesAmountInColumn;

        foreach (GameObject gObj in currentObjInCollider)
        {
            ObjectParameters objParameters = gObj.GetComponent<ObjectParameters>();

            // Jeżeli jest w rzędzie odpowiadającym wciśniętej liczbie
            if (objParameters.ID < entitiesInColumn * column && objParameters.ID >= (entitiesInColumn * column) - entitiesInColumn)
            {
                float timer = GetComponentInParent<LevelManager>().timer; // Aktualny czas
                if (timer > objParameters.actionStartTime && timer < objParameters.actionEndTime)
                {
                    print("Eureka! Start: " + objParameters.actionStartTime + ", End: " + objParameters.actionEndTime + ", Time: " + timer);
                    currentObjInCollider.Remove(gObj);
                    Destroy(gObj);
                }
                break;
            }
        }
    }
}
