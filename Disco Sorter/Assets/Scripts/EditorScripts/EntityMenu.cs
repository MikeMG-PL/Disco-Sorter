using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    [Header("Poszczególne elementy menu: ")]
    [SerializeField]
    private Dropdown typeDropdown;          // Dropdown, w którym wybiera się typ obiektu i dropdown wyboru koloru
    [SerializeField]
    private Dropdown colorDropdown;
    [SerializeField]
    private Dropdown actionDropdown;

    [Header("Kolor obiektu w edytorze: ")]
    public Color noColor;
    public Color apple1Color;
    public Color apple2Color;

    [Header("Ikona obiektu w edytorze: ")]
    public GameObject apple;
    public GameObject rottenApple;
    public GameObject disco;

    [Header("Obiekt zawierający skrypt SongController")]
    [SerializeField]
    private GameObject songController;      // Obiekt SongController, który ma w sobie skrypt EditorNet, który to z kolei jest potrzebny do pozyskania tablicy entities

    [Header("Panel menu")]
    [SerializeField]
    private GameObject menuPanel;           // Panel z całym menu właściowości obiektu

    [Header("Ostrzeżenie o niemożności wyboru koloru")]
    [SerializeField]
    private Text colorWarning;              // Tekst mówiący, że tylko określonym obiektom można dodać kolor
    [SerializeField]
    private Text actionWarning;             // Tekst mówiący, że tylko określonym obiektom można dostosować rodzaj akcji

    private GameObject[] entityArray;       // Tablica z entities
    private MenuManager menuManager;
    private int currentEntity = -1;         // Aktualnie zaznaczony obiekt
    private int previousEntity;             // Poprzednio zaznaczony obiekt

    void Start()
    {
        menuPanel.SetActive(false);
        menuManager = GetComponent<MenuManager>();
    }

    // Funkcja przypisuje 
    public void Initialization()
    {
        entityArray = songController.GetComponent<EditorNet>().entityArray;

        // Każdemu obiektowi z tablicy przypisujemy ten skrypt, aby mógł zarządzać menu jego właściwości
        for (int i = 0; i < entityArray.Length; i++)
        {
            entityArray[i].GetComponent<Entity>().entityMenuScript = this;
        }

        CloseMenu();
    }

    // Otwarcie menu po kliknięciu jakiegoś obiektu, wyróżnienie obiektu, który został wybrany, "odwyróżnienie" poprzedniego obiektu
    public void OpenMenu(int entityNumber)
    {
        // Panel z menu obiektu jest teraz aktywnym panelem
        menuManager.ChangeActivePanel(menuPanel);

        previousEntity = currentEntity;
        currentEntity = entityNumber;

        SetCurrentValues();

        // Wyróżnianie nowego obiektu i odwyróżnianie poprzedniego (jeśli jakiś był)
        if (previousEntity != -1)
            entityArray[previousEntity].GetComponent<Entity>().Highlight(false);
        entityArray[currentEntity].GetComponent<Entity>().Highlight(true);
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
    public void ChangeType(int entityType)
    {
        Entity entity = entityArray[currentEntity].GetComponent<Entity>();
        entity.entityType = entityType;
        entity.ChangeTypeIcon();
        SetCurrentValues();
    }

    // Zmiana właściwości obiektu: kolor
    public void ChangeColor(int color)
    {
        Entity entity = entityArray[currentEntity].GetComponent<Entity>();
        entity.color = color;
        entity.ChangeColor();
        SetCurrentValues();
    }

    public void ChangeAction(int action)
    {
        entityArray[currentEntity].GetComponent<Entity>().action = action;
        SetCurrentValues();
    }

    // Funkcje pomocnicze
    // Panel menu jest jeden. Dla każdego obiektu tuż przed otwarciem ustawiane są w nim wartości, które odpowiadają wybranemu właśnie obiektowi.
    private void SetCurrentValues()
    {
        Entity entity = entityArray[currentEntity].GetComponent<Entity>();

        StillHasColor(entity);
        StillHasAction(entity);

        typeDropdown.value = entity.entityType;
        colorDropdown.value = entity.color;
        actionDropdown.value = entity.action;
    }

    // Sprawdza czy obiektowi o wybranym typie można zmienić kolor
    private void StillHasColor(Entity entity)
    {
        if (entity.entityType == 1)
        {
            colorDropdown.interactable = true;
            colorWarning.gameObject.SetActive(false);
        }

        else
        {
            entity.color = 0;
            colorDropdown.interactable = false;
            colorWarning.gameObject.SetActive(true);
        }
    }

    // Sprawdza czy obiektowi o wybranym typie można zmienić akcję
    private void StillHasAction(Entity entity)
    {
        if (entity.entityType == 1 || entity.entityType == 2)
        {
            actionDropdown.interactable = true;
            actionWarning.gameObject.SetActive(false);
        }

        else
        {
            entity.action = 0;
            actionDropdown.interactable = false;
            actionWarning.gameObject.SetActive(true);
        }
    }
}
