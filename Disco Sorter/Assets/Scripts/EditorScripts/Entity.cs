using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{

    public GameObject markerPrefab;                     // Prefab znacznika
    GameObject marker;
    public int entityNumber;                            // Numer (identyfikator) obiektu
    public int entityType;                              // Typ obiektu
    public int color;                                   // Kolory jabłek, 0 - brak, 1 - zielony, 2 - czerwony
    [HideInInspector]
    public EntityMenu entityMenuScript;

    private bool highlighted;                           // Czy obiekt jest aktualnie zaznaczony przez użytkownika
    // Przydałoby się to może jakoś inaczej zrobić, minHight pobierać na Start a maxHight wybierać gdzieś w EditorNet? Nieistotne na razie.
    private float maxHightOfHighlight = -0.03f;         // Wysokość, na jaką wzniesie się zaznaczony obiekt
    private float minHight = -0.15f;                    // Wysokość standardowa niezaznaczonego obiektu

    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() upewnia się, że użytkownik nie kliknął na obiekt przez jakiś element UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (marker == null)
                marker = Instantiate(markerPrefab, transform.position, transform.rotation);
            // Otwieranie menu obiektu, w którym można dostować jego właściwości
            entityMenuScript.OpenMenu(entityNumber);
        }
    }

    // Przełącza wyróżnienie obiektu, wykorzystuje to skrypt EntityMenu

    private void Update()
    {
        HighlightMark();
        //HighlightMove();
    }

    public void Highlight(bool highlight)
    {
        highlighted = highlight;
    }

    void HighlightMark()
    {
        if (highlighted)
        {

            if (marker.transform.localPosition.y < maxHightOfHighlight)
                marker.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            else
                marker.transform.localPosition = new Vector3(transform.localPosition.x, maxHightOfHighlight, transform.localPosition.z);
        }
        else
            Destroy(marker);
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

    // Zwraca kolor, który powinien mieć obiekt w edytorze
    public Color GetColor()
    {
        if (color == 0)
            return Color.white;
        else if (color == 1)
            return Color.green;
        else if (color == 2)
            return Color.red;
        else return Color.white;
    }
}
