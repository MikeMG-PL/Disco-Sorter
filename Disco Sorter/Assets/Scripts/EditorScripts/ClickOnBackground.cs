using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnBackground : MonoBehaviour
{
    public Canvas entityMenu;           // Menu właściwości obiektu
    public GameObject savesPanel;       // Panel z zapisami

    // Gdy klikniemy na tło, aktywne menu zostaną zamknięte
    private void OnMouseDown()
    {
        // EventSystem.current.IsPointerOverGameObject() sprawdza czy nie kliknęliśmy na UI
        if (!EventSystem.current.IsPointerOverGameObject() && !Input.GetKey(KeyCode.LeftControl))
        {
            entityMenu.GetComponent<EntityMenu>().CloseMenu();
            savesPanel.SetActive(false);
        }
    }
}
