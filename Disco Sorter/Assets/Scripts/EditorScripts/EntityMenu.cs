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

    [SerializeField]
    private Image isSavedImage;
    [SerializeField]
    private Sprite savedImage;
    [SerializeField]
    private Sprite unsavedImage;

    [HideInInspector]
    public List<GameObject> markedEntities = new List<GameObject>();

    private GameObject[] entityArray;       // Tablica z entities
    private MenuManager menuManager;

    void Start()
    {
        menuPanel.SetActive(false);
        menuManager = GetComponent<MenuManager>();
    }

    // Funkcja przypisuje 
    public void Initialization()
    {
        entityArray = songController.GetComponent<EditorNet>().entityArray;

        // Każdemu obiektowi z tablicy przypisujemy ten skrypt
        for (int i = 0; i < entityArray.Length; i++)
        {
            entityArray[i].GetComponent<Entity>().entityMenuScript = this;
        }

        // Zamyka menu na wypadek gdyby było otwarte (np. po rebuildzie siatki)
        CloseMenu();
    }

    // Otwarcie menu po kliknięciu jakiegoś obiektu, wyróżnienie obiektu, który został wybrany
    public void OpenMenu(int entityNumber)
    {
        // Panel z menu obiektu jest teraz aktywnym panelem
        menuManager.ChangeActivePanel(menuPanel);

        // Ustala wartości dropdown'ów w panelu na odpowiadające aktualnemu obiektowi, jeśli zaznaczone jest wiele obiektów, panel pokazuje domyślne wartości dropdown'ów (czyli "None")
        if (markedEntities.Count == 1)
            SetCurrentValues(entityNumber);
        else SetBlank();

        // Zaznacza markerem wybrany obiekt
        entityArray[entityNumber].GetComponent<Entity>().Highlight(true);
    }

    // Zamykanie menu, odwyróżnianie obiektów
    public void CloseMenu()
    {
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
            DeleteAllMarks();
        }
    }

    // Niszczy wszystkie markery
    public void DeleteAllMarks()
    {
        for (int i = 0; i < markedEntities.Count; i++)
        {
            markedEntities[i].GetComponent<Entity>().Highlight(false);
        }
    }

    // Funkcje wykorzystywane bezpośrednio przez menu
    // Po wybraniu konkretnej opcji można zmieniać wygląd kostki aby w jakiś sposób to zasygnalizować
    // Zmiana właściwości obiektu: typ
    public void ChangeType(int entityType)
    {
        IsSavedChange(false);

        for (int i = 0; i < markedEntities.Count; i++)
        {
            Entity entity = markedEntities[i].GetComponent<Entity>();

            entity.entityType = entityType;
            entity.ChangeTypeIcon();
            SetCurrentValues(entity.entityNumber);
        }
    }

    // Zmiana właściwości obiektu: kolor
    public void ChangeColor(int color)
    {
        IsSavedChange(false);

        for (int i = 0; i < markedEntities.Count; i++)
        {
            Entity entity = markedEntities[i].GetComponent<Entity>();

            entity.color = color;
            entity.ChangeColor();
            SetCurrentValues(entity.entityNumber);
        }
    }

    public void ChangeAction(int action)
    {
        IsSavedChange(false);

        for (int i = 0; i < markedEntities.Count; i++)
        {
            Entity entity = markedEntities[i].GetComponent<Entity>();

            entity.action = action;
            SetCurrentValues(entity.entityNumber);
        }
    }

    // Funkcje pomocnicze
    // Panel menu jest jeden. Dla każdego obiektu tuż przed otwarciem ustawiane są w nim wartości, które odpowiadają wybranemu właśnie obiektowi.
    private void SetCurrentValues(int entityNumber)
    {
        Entity entity = entityArray[entityNumber].GetComponent<Entity>();

        StillHasColor(entity);
        StillHasAction(entity);

        typeDropdown.SetValueWithoutNotify(entity.entityType);
        colorDropdown.SetValueWithoutNotify(entity.color);
        actionDropdown.SetValueWithoutNotify(entity.action);
    }

    // Ustawia wartości na domyślne (None) i blokuje, w związku z tym, niektóre opcje
    private void SetBlank()
    {
        colorDropdown.interactable = false;
        colorWarning.gameObject.SetActive(true);
        actionDropdown.interactable = false;
        actionWarning.gameObject.SetActive(true);

        typeDropdown.SetValueWithoutNotify(0);
        colorDropdown.SetValueWithoutNotify(0);
        actionDropdown.SetValueWithoutNotify(0);
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
            entity.ChangeColor();
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

    // Zamienia ikonkę mówiącą użytkownikowi czy aktualna siatka jest zapisana, czy nie
    public void IsSavedChange(bool saved)
    {
        if (saved) isSavedImage.sprite = savedImage;
        else isSavedImage.sprite = unsavedImage;
    }
}
