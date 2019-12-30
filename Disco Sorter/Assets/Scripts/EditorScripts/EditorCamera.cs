using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    public float cameraSpeed = 3.5f;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private float scrollInput;

    void Update()
    {
        /*
        yaw += speedH * Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0f, yaw, 0f);
        */

        CameraMove();
    }
    


    // Odpowiada za ruch kamery za pomocą scrolla
    void CameraMove()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scrollInput * cameraSpeed, Space.Self);
    }
}
