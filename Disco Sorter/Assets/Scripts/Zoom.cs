using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{
    EditorCamera editorCamera;                  // Trzymadełko kamery - skrypt EditorCamera
    Camera cam;                                 // Komponent kamery (ma go dziecko trzymadełka - właściwa kamera)
    float orthographicSize, scrollInput;        // Początkowa wartość orthographicSize; Wartość wychylenia scrolla
    public Slider slider;                       // Slider przybliżenia

    void Start()
    {
        editorCamera = GetComponent<EditorCamera>();                                                // Nadanie komponentów
        cam = gameObject.transform.GetChild(0).GetComponent<Camera>();                              // j.w.

        orthographicSize = cam.orthographicSize;                                                    // Zapamiętanie początkowej wartości orthographicSize

        slider.value = (cam.orthographicSize - 0.5f * orthographicSize) / 4f * orthographicSize;    // Przeniesienie początkowej wartości orthographicSize na slider (poprzez wzór matematyczny)
    }

    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");                                            // Pobieranie wartości wychylenia scrolla

        if (!editorCamera.moveCamera)                                                                // Jeśli kamera centruje się na znaczniku, użytk. przybliża za pomocą scrolla
            slider.value -= scrollInput * 0.5f;

        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ||              // W przeciwnym wypadku - za pomocą przytrzymania jednego z poniższych klawiszy + SCROLL
                 Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftControl) ||
                 Input.GetKey(KeyCode.RightControl))

        { editorCamera.moveCamSwitch = false; slider.value -= scrollInput * 0.5f; }
                                                                                                        
        else
            editorCamera.moveCamSwitch = true;                                                       // Przy puszczeniu przycisku użyk. nie może przybliżać, scrollem przemieszcza kamerę po długości

    }

    /// FUNKCJA OBSŁUGUJĄCA ZOOM RĘCZNY ZA POMOCĄ SLIDERA ///
    public void Zooming()
    {
        if (cam != null)
            cam.orthographicSize = orthographicSize * (slider.GetComponent<Slider>().value * 4f + 0.5f);
    }


}
