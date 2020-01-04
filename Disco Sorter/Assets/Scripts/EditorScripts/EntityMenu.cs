﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenu : MonoBehaviour
{
    // Poszczególne elementy menu
    public Dropdown typeDropdown;           // Dropdown, w którym wybiera się typ obiektu
    public List<string> entityTypes;        // Wszystkie typy obiektów, umieszczane są w typeDropdown
    public Dropdown colorDropdown;          // Dropdown, w którym wybiera się kolor obiektu
    public Text colorWarning;               // Tekst mówiący, że tylko jabłkom można dostować kolor

    public GameObject songController;       // Obiekt SongController, który ma w sobie skrypt EditorNet, który to z kolei jest potrzebny do pozyskania tablicy entities
    public GameObject menuPanel;            // Panel z całym menu właściowości obiektu

    private GameObject[] entityArray;       // Tablica z entities
    private int currentEntity = -1;         // Aktualnie zaznaczony obiekt
    private int previousEntity;             // Poprzednio zaznaczony obiekt

    void Start()
    {
        // Dodawanie wszystkich opcji, które ustalono w liście entityTypes bezpośrednio w edytorze
        typeDropdown.AddOptions(entityTypes);
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
        Entity entity = entityArray[currentEntity].GetComponent<Entity>();
        typeDropdown.value = entity.entityType;
        colorDropdown.value = entity.color;

        SetInteractableOptions(entity);
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
            Entity entity = entityArray[currentEntity].GetComponent<Entity>();
            entity.entityType = entityType;
            entityArray[currentEntity].GetComponent<Renderer>().material.color = entity.GetColor();
            SetInteractableOptions(entity);
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

    private void SetInteractableOptions(Entity entity)
    {
        if (entity.IsApple())
        {
            colorDropdown.interactable = true;
            colorWarning.gameObject.SetActive(false);
        }
        else
        {
            colorDropdown.interactable = false;
            colorWarning.gameObject.SetActive(true);
        }
    }
}
