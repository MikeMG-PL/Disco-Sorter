using UnityEngine;
using UnityEngine.UI;

public class EditorCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject toggle;
    Zoom zoom;
    [HideInInspector]
    public float cameraSpeed = 3.5f;
    public GameObject songController;
    public Slider slider;
    float scrollInput;
    Vector3 pos, newPos;

    void Start()
    {
        zoom = GetComponent<Zoom>();
        zoom.scrollZoom = false;
        SwitchBool();
    }

    void Update()
    {
        FollowMarker();
    }

    /// FUNKCJA WYWOŁYWANA PRZY ZAZNACZENIU PTASZKA, ZMIENIAJĄCA DZIAŁANIE SCROLLA ///
    public void SwitchBool()
    {
        zoom.scrollZoom = !zoom.scrollZoom;
    }

    /// FUNKCJA CENTRUJĄCA WIDOK NA ZNACZNIKU ///
    public void MoveToPoint(float x)
    {
        if (zoom.scrollZoom)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    /// FUNKCJA POWODUJĄCA PODĄŻANIE ZA ZNACZNIKIEM ///
    public void FollowMarker()
    {
        if (zoom.scrollZoom)
        {
            scrollInput = Input.GetAxis("Mouse ScrollWheel");
            slider.value -= scrollInput * 0.5f;


            GameObject currentEntity = songController.GetComponent<EntityCurrentTimeHighlight>().currentEntity;

            if (currentEntity != null)
                pos = currentEntity.transform.position;

            newPos = new Vector3(pos.x, transform.position.y, pos.z);

            transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
        }
    }
}
