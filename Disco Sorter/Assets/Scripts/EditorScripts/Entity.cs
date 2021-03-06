﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    // Zmienne określające dany obiekt, zapisywane są one do pliku
    public EntityAction action;                                  // Akcja, którą można wykonać na obiekcie
    public int entityNumber;                            // Numer (identyfikator) obiektu
    public EntityType type;                             // Typ obiektu
    public EntityColour color;                          // Kolory jabłek, 0 - brak, 1 - zielony, 2 - czerwony
    public int linkedReleaseEN = -1;                    // Numer entity, do którego należy wyrzucić ten obiekt
    public int linkedCatchEN = -1;                      // Numer entity, który trzeba wyrzucić do tego obiektu
    public float linkedReleaseTimeStart = -1;
    public float linkedReleaseTimeEnd = -1;
    public float linkedCatchTime = -1;

    [HideInInspector]
    public EntityMenu entityMenuScript;

    [SerializeField]
    private GameObject markerPrefab;                    // Prefab znacznika

    private GameObject marker, icon, actionIcon;
    private bool isHighlighted;                         // Czy obiekt jest aktualnie zaznaczony przez użytkownika

    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() upewnia się, że użytkownik nie kliknął na obiekt przez jakiś element UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Jeśli użytkownik ustala właśnie punkt release, oraz jeśli obiekt ten nie jest już punktem release dla innego entity, ani punktem catch
            if (entityMenuScript.isSettingRelease)
            {
                if (action != EntityAction.CatchAndRelease && action != EntityAction.ReleasePoint)
                    entityMenuScript.gameObject.GetComponent<SetCatchRelease>().SetReleaseEntity(this);
            }

            else
            {
                // Jeśli obiekt, który wskazał użytkownik jest punktem release, kamera wskazuje mu obiekt catch, połączony z tym obiektem
                if (action == EntityAction.ReleasePoint)
                {
                    entityMenuScript.PointCatchEntity(linkedCatchEN);
                }

                // Jeśli żadna z powyższych opcji nie jest wywoływana
                else
                {
                    // Jeśli użytkownik trzyma lewy ctrl - zaznacza wiele obiektów do zmiany. Jeśli nie to znaczy, że zaznaczył nowy, pojedynczy obiekt
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        entityMenuScript.DeleteAllMarks();
                        entityMenuScript.markedEntities.Clear();
                    }

                    // Jeśli użytkownik zaznaczył obiekt, który już jest na liście, obiekt ten jest usuwany z listy i odznaczany
                    if (entityMenuScript.markedEntities.Contains(gameObject))
                    {
                        Highlight(false);
                        entityMenuScript.markedEntities.Remove(gameObject);
                        return;
                    }

                    entityMenuScript.markedEntities.Add(gameObject);

                    // Otwieranie menu obiektu, w którym można dostować jego właściwości
                    entityMenuScript.OpenMenu(entityNumber);
                }
            }
        }
    }

    private void OnMouseOver()
    {
        // Jeśli kliknięto prawym przyciskiem myszy na punkt release
        if (action == EntityAction.ReleasePoint && Input.GetMouseButtonDown(1))
        {
            entityMenuScript.gameObject.GetComponent<SetCatchRelease>().SetUnreleaseEntity(this);
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

    // Zaznacza i otwiera menu tego konkretnie Entity, usuwając z listy pozostałe
    public void OpenThisEntityMenu()
    {
        entityMenuScript.DeleteAllMarks();
        entityMenuScript.markedEntities.Clear();
        entityMenuScript.markedEntities.Add(gameObject);
        entityMenuScript.OpenMenu(entityNumber);
    }

    // Zmienia kolor obiektu w edytorze, na ustalony w skrypcie EntityMenu
    public void ChangeColor()
    {
        // Jeśli obiekt jest typem none, lub nie ma wybranego koloru zwracany jest kolor biały
        if (color == EntityColour.None)
            GetComponent<Renderer>().material.color = entityMenuScript.noColor;
        else if (color == EntityColour.Green)
            GetComponent<Renderer>().material.color = entityMenuScript.apple1Color;
        else if (color == EntityColour.Red)
            GetComponent<Renderer>().material.color = entityMenuScript.apple2Color;
        else GetComponent<Renderer>().material.color = entityMenuScript.noColor;
    }

    // Zmienia ikonkę nad obiektem, na ustaloną w skrypcie EntityMenu, biorąc pod uwagę typ entity
    public void ChangeTypeIcon()
    {
        Destroy(icon);
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

        if (type == EntityType.Apple)
            icon = Instantiate(entityMenuScript.apple, position, entityMenuScript.apple.transform.rotation, transform);
        else if (type == EntityType.RottenApple)
            icon = Instantiate(entityMenuScript.rottenApple, position, entityMenuScript.rottenApple.transform.rotation, transform);
        else if (type == EntityType.Disco)
            icon = Instantiate(entityMenuScript.disco, position, entityMenuScript.disco.transform.rotation, transform);
    }

    // Zmienia ikonkę nad obiektem, na ustaloną w skrypcie EntityMenu, biorąc pod uwagę action entity
    public void ChangeActionIcon()
    {
        Destroy(actionIcon);
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

        if (action == EntityAction.ReleasePoint)
        {
            actionIcon = Instantiate(entityMenuScript.releaseEntity, position, entityMenuScript.releaseEntity.transform.rotation, transform);
        }
    }
}
