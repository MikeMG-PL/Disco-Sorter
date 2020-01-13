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
    private bool isHighlighted;                         // Czy obiekt jest aktualnie zaznaczony przez użytkownika

    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() upewnia się, że użytkownik nie kliknął na obiekt przez jakiś element UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Otwieranie menu obiektu, w którym można dostować jego właściwości

            // Jeśli użytkownik trzyma lewy ctrl, zaznacza wiele obiektów do zmiany. Jeśli nie to znaczy, że zaznaczył nowy, pojedynczy obiekt
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                entityMenuScript.DeleteAllMarks();
                entityMenuScript.markedEntities.Clear();
            }

            // Jeśli użytkownik zaznaczył obiekt, którego jeszcze nie ma na liście
            if (!entityMenuScript.markedEntities.Contains(gameObject))
                entityMenuScript.markedEntities.Add(gameObject);

            entityMenuScript.OpenMenu(entityNumber);
        }
    }

    // Tworzy marker nad obiektem wybranym przez użytkownika
    public void Highlight(bool highlight)
    {
        // Jeśli obiekt jeszcze nie jest zaznaczony, a użytkownik go zaznaczył
        if (!isHighlighted && highlight)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            marker = Instantiate(markerPrefab, position, Quaternion.identity, transform);
            isHighlighted = true;
        }

        // Jeśli obiekt jest zaznaczony, a użytkownik go "odznaczył" (kliknął gdzieś)
        else if (isHighlighted && !highlight)
        {
            Destroy(marker);
            isHighlighted = false;
        }
    }

    // Zmienia kolor obiektu w edytorze, na ustalony w skrypcie EntityMenu
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

    // Zmienia ikonkę nad obiektem, na ustaloną w skrypcie EntityMenu
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
