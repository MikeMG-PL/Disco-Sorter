using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 3.5f;

    private float scrollInput;

    void Update()
    {
        CameraMove();
    }

    // Odpowiada za ruch kamery za pomocą scrolla
    void CameraMove()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scrollInput * cameraSpeed, Space.Self);
    }
}
