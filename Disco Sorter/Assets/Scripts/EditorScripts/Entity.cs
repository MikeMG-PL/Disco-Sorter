using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    // Zmienne określające dany obiekt, zapisywane są one do pliku
    public int action;                                  // Akcja, którą można wykonać na obiekcie
    public int entityNumber;                            // Numer (identyfikator) obiektu
    public int entityType;                              // Typ obiektu
    public int color;                                   // Kolory jabłek, 0 - brak, 1 - zielony, 2 - czerwony

    [HideInInspector]
    public EntityMenu entityMenuScript;

    [SerializeField]
    private GameObject markerPrefab;                    // Prefab znacznika

    private GameObject marker, icon;
    private bool ishighlighted;                         // Czy obiekt jest aktualnie zaznaczony przez użytkownika

    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() upewnia się, że użytkownik nie kliknął na obiekt przez jakiś element UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Otwieranie menu obiektu, w którym można dostować jego właściwości
            entityMenuScript.OpenMenu(entityNumber);
        }
    }

    public void Highlight(bool highlight)
    {
        ishighlighted = highlight;

        if (ishighlighted)
        {
            if (marker == null)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                marker = Instantiate(markerPrefab, position, Quaternion.identity, transform);
            }
        }
        else
            Destroy(marker);
    }

    // Zmienia kolor obiektu w edytorze, na ustalone w skrypcie EntityMenu
    public void ChangeColor()
    {
        // Jeśli obiekt jest typem none, lub nie ma wybranego koloru zwracany jest kolor biały
        if (color == 0)
            GetComponent<Renderer>().material.color = entityMenuScript.noColor;
        else if (color == 1)
            GetComponent<Renderer>().material.color = entityMenuScript.apple1Color;
        else if (color == 2)
            GetComponent<Renderer>().material.color = entityMenuScript.apple2Color;
        else GetComponent<Renderer>().material.color = entityMenuScript.noColor;
    }

    public void ChangeTypeIcon()
    {
        Destroy(icon);
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

        if (entityType == 1)
            icon = Instantiate(entityMenuScript.apple, position, entityMenuScript.apple.transform.rotation, transform);
        else if (entityType == 2)
            icon = Instantiate(entityMenuScript.rottenApple, position, entityMenuScript.apple.transform.rotation, transform);
        else if (entityType == 3)
            icon = Instantiate(entityMenuScript.disco, position, entityMenuScript.apple.transform.rotation, transform);
    }
}
