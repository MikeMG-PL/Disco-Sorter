using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 3.5f;
    public GameObject songController;
    private float scrollInput;
    private Vector3 pos;
    private Vector3 newPos;

    void Update()
    {
        CameraMove();
        FollowMarker();
    }

    // Odpowiada za ruch kamery za pomocą scrolla
    void CameraMove()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scrollInput * cameraSpeed, Space.Self);
    }

    public void MoveToPoint(float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void FollowMarker()
    {
        if (songController.GetComponent<EntityCurrentTimeHighlight>().currentEntity != null)
            pos = songController.GetComponent<EntityCurrentTimeHighlight>().currentEntity.transform.position;
        newPos = new Vector3(pos.x, gameObject.transform.GetChild(0).position.y, pos.z);

        gameObject.transform.GetChild(0).position = Vector3.Lerp(gameObject.transform.GetChild(0).position, newPos, 0.1f);
    }
}
