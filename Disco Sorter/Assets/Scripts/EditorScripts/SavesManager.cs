using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavesManager : MonoBehaviour
{
    // Tablica przycisków, które należą do panelu. Są one przypisane przez edytor, ale można by też dodawać je automatycznie
    public GameObject[] buttons;

    // Zmienia tekst każdego przycisku, na odpowiadającą nazwę piosenki
    public void UpdateSavesNames(string[] songNames)
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < songNames.Length)
                buttons[i].GetComponentInChildren<Text>().text = songNames[i];
            else
                buttons[i].GetComponentInChildren<Text>().text = "Puste";

            //Debug.Log(buttons[i].GetComponentInChildren<Text>().text);
        }
    }
}
