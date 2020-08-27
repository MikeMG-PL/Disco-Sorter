using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoColor : MonoBehaviour
{
    // TODO Jeśli nie chcemy aby kolory były ustawiane dynamiczne podczas sceny, to lepiej ustawić je ręcznie

    public enum LampColor { blue, green, pink, red, white, yellow };

    public LampColor color;
    bool thisIsMaluch, thisIsLightbar;

    public List<Material> materials = new List<Material>();
    public List<Material> fogs = new List<Material>();

    void Awake()
    {
        switch(gameObject.tag)
        {
            case "Lightbar":
                thisIsLightbar = true;
                thisIsMaluch = false;
                break;
            case "Maluch":
                thisIsLightbar = false;
                thisIsMaluch = true;
                break;
            default:
                thisIsLightbar = false;
                thisIsMaluch = false;
                Debug.LogError("There is an object in the scene which has neither \"Maluch\" nor \"Lightbar\" tag.");
                break;
        }

        if (thisIsMaluch)
            SetColorMaluch(color);
        else if (thisIsLightbar)
            SetColorLightbar(color);
    }

    void SetColorMaluch(LampColor c)
    {
        transform.GetChild(2).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = fogs[(int)color];
        transform.GetChild(2).GetChild(1).GetComponent<MeshRenderer>().material = materials[(int)color];
    }

    void SetColorLightbar(LampColor c)
    {
        transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(0).GetChild(5).GetComponent<MeshRenderer>().material = materials[(int)color];
        transform.GetChild(0).GetChild(6).GetComponent<MeshRenderer>().material = materials[(int)color];
    }

}

// Material.setFloat(„_Color”, new Color())