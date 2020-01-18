using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{
    EditorCamera editorCamera;                  // Trzymadełko kamery - skrypt EditorCamera
    Camera cam;                                 // Komponent kamery (ma go dziecko trzymadełka - właściwa kamera)
    float orthographicSize, scrollInput;        // Początkowa wartość orthographicSize; Wartość wychylenia scrolla
    [HideInInspector]
    public Slider slider;                       // Slider przybliżenia
    [HideInInspector]
    public bool scrollZoom;                     // Zmienna-przełącznik służąca do przełączania zadania scrolla

    void Start()
    {
        Init();
    }

    void Update()
    {
        ScrollFunctions();
        HoldShift();
    }

    /// FUNKCJA OBSŁUGUJĄCA ZOOM RĘCZNY ZA POMOCĄ SLIDERA ///
    public void Zooming()
    {
        if (cam != null)
            cam.orthographicSize = orthographicSize * (slider.GetComponent<Slider>().value * 4f + 0.5f);
    }

    /// FUNKCJA OBSŁUGUJĄCA TO, CO ROBI SCROLL ///
    void ScrollFunctions()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        switch (scrollZoom)
        {
            case true:
                slider.value -= scrollInput * 0.5f;
                break;

            case false:
                transform.Translate(0, 0, scrollInput * editorCamera.cameraSpeed, Space.Self);
                break;
        }
    }

    /// FUNKCJA ZMIENIAJĄCA DZIAŁANIE SCROLLA PO PRZYTRZYMANIU SHIFTA ///
    void HoldShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)
            || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))

        { scrollZoom = !scrollZoom; }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)
            || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))

        { scrollZoom = !scrollZoom; }
    }

    /// INICJALIZACJA KOMPONENTÓW, WCZYTANIE SLIDERÓW... ///
    void Init()
    {
        editorCamera = GetComponent<EditorCamera>();
        cam = gameObject.transform.GetChild(0).GetComponent<Camera>();
        orthographicSize = cam.orthographicSize;
        slider.value = (cam.orthographicSize - 0.5f * orthographicSize) / 4f * orthographicSize;
    }
}
