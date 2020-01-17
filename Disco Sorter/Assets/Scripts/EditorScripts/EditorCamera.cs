using UnityEngine;
using UnityEngine.UI;

public class EditorCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject toggle;
    [SerializeField]
    private float cameraSpeed = 3.5f;
    public GameObject songController;
    public bool moveCamera, moveCamSwitch = true; // moveCamSwitch służy do wyłączania ruchu kamery wzdłuż za pomocą scrolla, gdy przybliżamy kamerę poprzez np. LSHIFT + SCROLL
    private float scrollInput;
    private Vector3 pos;
    private Vector3 newPos;

    void Update()
    {
        if (moveCamera) // Jeśli gracz sam chce poruszać kamerą
            CameraMove();
        else
            FollowMarker(); // Jeśli kamera ma się centrować na znaczniku
    }

    public void SwitchBool()
    {
        moveCamera = !moveCamera;
    }

    // Odpowiada za ruch kamery za pomocą scrolla
    private void CameraMove()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (moveCamSwitch)                                                          // Jeśli nie ma kombinacji klawiszy np. LSHIFT + SCROLL, służącej do przybliżania, to można poruszać kamerą
            transform.Translate(0, 0, scrollInput * cameraSpeed, Space.Self);
    }

    public void MoveToPoint(float x)
    {
        toggle.GetComponent<Toggle>().isOn = false;     // można spróbować te 2 linijki napisać bardziej elegancko, jest tu obecnie podejście YOLO przy robieniu UI
        moveCamera = true;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void FollowMarker()
    {
        GameObject currentEntity = songController.GetComponent<EntityCurrentTimeHighlight>().currentEntity;

        if (currentEntity != null)
            pos = currentEntity.transform.position;

        newPos = new Vector3(pos.x, transform.position.y, pos.z);

        transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
    }
}
