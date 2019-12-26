using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    public GameObject songController;       // Obiekt SongController, który ma w sobie skrypt EditorNet, który to z kolei jest potrzebny do pozyskania tablicy entities
    public GameObject menuPanel;            // Panel z całym menu właściowości obiektu
    public Dropdown typeDropdown;           // Poszczególne elementy panelu, po pierwsze: dropdown, w którym wybiera się typ obiektu

    public List<string> entityTypes;        // Wszystkie typy obiektów

    private GameObject[] entityArray;       // Tablica z entities
    private int currentEntity = -1;         // Aktualnie zaznaczony obiekt
    private int previousEntity;             // Poprzednio zaznaczony obiekt

    void Start()
    {
        entityArray = songController.GetComponent<EditorNet>().entityArray;

        // Każdemu obiektowi z tablicy przypisujemy ten skrypt, aby mógł zarządzać menu jego właściwości
        for (int i = 0; i < entityArray.Length - 1; i++)
        {
            entityArray[i].GetComponent<Entity>().entityMenuScript = this;
        }

        // Dodawanie wszystkich opcji, które ustalono w liście entityTypes bezpośrednio w edytorze
        typeDropdown.AddOptions(entityTypes);
    }

    // Otwarcie menu, wyróżnienie obiektu, "odwyróżnienie" poprzedniego obiektu
    public void OpenMenu(int entityNumber)
    {
        // Aktywowanie panelu z menu
        menuPanel.SetActive(true);

        previousEntity = currentEntity;
        currentEntity = entityNumber;

        SetCurrentValues();

        // Wyróżnianie nowego obiektu i odwyróżnianie poprzedniego (jeśli jakiś był)
        if (previousEntity != -1)
            entityArray[previousEntity].GetComponent<Entity>().Highlight();
        entityArray[currentEntity].GetComponent<Entity>().Highlight();
    }

    // Panel - menu jest jedno. Dla każdego obiektu ustawiane są wartości, które powinno pokazać menu
    private void SetCurrentValues()
    {
        typeDropdown.value = entityArray[currentEntity].GetComponent<Entity>().entityType;
    }

    // Zamykanie menu i odwyróżnianie obiektu
    public void CloseMenu()
    {
        if (menuPanel.activeSelf)
        {
            entityArray[currentEntity].GetComponent<Entity>().Highlight();
            menuPanel.SetActive(false);
            currentEntity = -1;
        }
    }

    // Funkcje wykorzystywane bezpośrednio przez menu
    // Zmiana właściwości obiektu: typ
    public void ChangeEntityType(int entityType)
    {
        if (currentEntity != -1)
        {
            entityArray[currentEntity].GetComponent<Entity>().entityType = entityType;
            Debug.Log(entityArray[currentEntity].GetComponent<Entity>().entityType);
        }
    }
}
