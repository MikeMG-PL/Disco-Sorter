﻿using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 3.5f;
    public GameObject songController;
    public bool followMarkerSwitch;
    private float scrollInput;
    private Vector3 pos;
    private Vector3 newPos;

    void Update()
    {
        if (followMarkerSwitch) // Jeśli gracz odznaczył podążanie kamery za znacznikiem
            CameraMove();
        else
            FollowMarker(); // Jeśli gracz zaznaczył podążanie kamery za znacznikiem
    }

    public void SwitchBool()
    {
        followMarkerSwitch = !followMarkerSwitch;
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
        GameObject currentEntity = songController.GetComponent<EntityCurrentTimeHighlight>().currentEntity;
        Transform child = gameObject.transform.GetChild(0);

        if (currentEntity != null)
            pos = currentEntity.transform.position;

        newPos = new Vector3(pos.x, child.position.y, pos.z);

        child.position = Vector3.Lerp(child.position, newPos, 0.1f);
    }
}
