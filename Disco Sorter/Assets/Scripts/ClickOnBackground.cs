using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnBackground : MonoBehaviour
{
    public Canvas entityMenu;

    // Gdy klikniemy na tło, aktywne menu zostanie zamknięte
    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() sprawdza czy nie kliknęliśmy na UI
        if (!EventSystem.current.IsPointerOverGameObject())
            entityMenu.GetComponent<EntityMenu>().CloseMenu();
    }
}
