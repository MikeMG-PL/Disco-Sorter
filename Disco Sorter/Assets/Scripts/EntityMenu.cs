using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    public GameObject songController;       // Obiekt SongController, który ma w sobie skrypt EditorNet, który to z kolei jest potrzebny do pozyskania tablicy entities
    public GameObject menuPanel;            // Panel z całym menu właściowości obiektu
    // Poszczególne elementy panelu
    public Dropdown typeDropdown;           // Dropdown, w którym wybiera się typ obiektu
    public List<string> entityTypes;        // Wszystkie typy obiektów, umieszczane są w typeDropdown
    public Dropdown colorDropdown;          // Dropdown, w którym wybiera się kolor obiektu

    private GameObject[] entityArray;       // Tablica z entities
    private int currentEntity = -1;         // Aktualnie zaznaczony obiekt
    private int previousEntity;             // Poprzednio zaznaczony obiekt

    void Start()
    {
        entityArray = songController.GetComponent<EditorNet>().entityArray;

        // Każdemu obiektowi z tablicy przypisujemy ten skrypt, aby mógł zarządzać menu jego właściwości
        for (int i = 0; i < entityArray.Length; i++)
        {
            entityArray[i].GetComponent<Entity>().entityMenuScript = this;
        }

        // Dodawanie wszystkich opcji, które ustalono w liście entityTypes bezpośrednio w edytorze
        typeDropdown.AddOptions(entityTypes);
    }

    // Otwarcie menu po kliknięciu jakiegoś obiektu, wyróżnienie obiektu, który został wybrany, "odwyróżnienie" poprzedniego obiektu
    public void OpenMenu(int entityNumber)
    {
        // Aktywowanie panelu z menu
        menuPanel.SetActive(true);

        previousEntity = currentEntity;
        currentEntity = entityNumber;

        SetCurrentValues();

        // Wyróżnianie nowego obiektu i odwyróżnianie poprzedniego (jeśli jakiś był)
        if (previousEntity != -1)
            entityArray[previousEntity].GetComponent<Entity>().Highlight(false);
        entityArray[currentEntity].GetComponent<Entity>().Highlight(true);
    }

    // Panel menu jest jeden. Dla każdego obiektu tuż przed otwarciem ustawiane są w nim wartości, które odpowiadają wybranemu właśnie obiektowi.
    private void SetCurrentValues()
    {
        typeDropdown.value = entityArray[currentEntity].GetComponent<Entity>().entityType;
        colorDropdown.value = entityArray[currentEntity].GetComponent<Entity>().color;
    }

    // Zamykanie menu, odwyróżnianie obiektu i ustawianie currentEntity na -1
    public void CloseMenu()
    {
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
            entityArray[currentEntity].GetComponent<Entity>().Highlight(false);
            currentEntity = -1;
        }
    }

    // Funkcje wykorzystywane bezpośrednio przez menu
    // Po wybraniu konkretnej opcji można zmieniać wygląd kostki aby w jakiś sposób to zasygnalizować
    // Zmiana właściwości obiektu: typ
    public void ChangeEntityType(int entityType)
    {
        if (currentEntity != -1)
        {
            entityArray[currentEntity].GetComponent<Entity>().entityType = entityType;
            //Debug.Log(entityArray[currentEntity].GetComponent<Entity>().entityType);
        }
    }

    // Zmiana właściwości obiektu: kolor
    public void ChangeColor(int color)
    {
        if (currentEntity != -1)
        {
            entityArray[currentEntity].GetComponent<Entity>().color = color;
            entityArray[currentEntity].GetComponent<Renderer>().material.color = entityArray[currentEntity].GetComponent<Entity>().GetColor();
            //Debug.Log(entityArray[currentEntity].GetComponent<Entity>().color);
        }
    }
}
