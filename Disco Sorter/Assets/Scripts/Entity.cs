using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{ 
    public int entityNumber;                            // Numer (identyfikator) obiektu
    public int entityType;                              // Typ obiektu
    [HideInInspector]
    public EntityMenu entityMenuScript;

    private bool highlighted;                           // Czy obiekt jest aktualnie zaznaczony
    private float maxHightOfHighlight = -0.03f;         // Wysokość, na jaką wzniesie się zaznaczony obiekt
    private float minHight = -0.15f;                    // Wysokość standardowa niezaznaczonego obiektu

    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() upewnia się, że użytkownik nie kliknął na obiekt przez jakiś element UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Otwieranie menu obiektu, w którym można dostować jego właściwości
            entityMenuScript.OpenMenu(entityNumber);
        }
    }

    // Przełącza wyróżnienie obiektu, wykorzystuje to skrypt EntityMenu

    private void Update()
    {
        HighlightMove();
    }

    public void Highlight(bool highlight)
    {
        highlighted = highlight;
    }

    // Zajmuje się przemieszczaniem wyróżnionego obiektu w górę (lub po "odwyróżnieniu" - w dół)
    void HighlightMove()
    {
        if (highlighted && transform.localPosition.y < maxHightOfHighlight)
        {
            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }

        else if (highlighted && transform.localPosition.y > maxHightOfHighlight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, maxHightOfHighlight, transform.localPosition.z);
        }

        else if (!highlighted && transform.localPosition.y > minHight)
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }

        else if (!highlighted && transform.localPosition.y < minHight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, minHight, transform.localPosition.z);
        }
    }
}
