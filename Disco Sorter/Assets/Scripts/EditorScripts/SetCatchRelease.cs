using UnityEngine;

public class SetCatchRelease : MonoBehaviour
{
    [SerializeField]
    private GameObject songController;

    private EditorNet editorNet;
    private EntityMenu entityMenuScript;
    private LineRenderer lineRenderer;

    private void Start()
    {
        editorNet = songController.GetComponent<EditorNet>();
        entityMenuScript = GetComponent<EntityMenu>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Zajmuje się rysowaniem za pomocą line renderer'a, jeśli aktualnie wybierany jest punkt release
        if (entityMenuScript.isSettingRelease)
        {
            Vector3 startPosition = entityMenuScript.catchEntity.gameObject.transform.position;
            startPosition.y = 0;
            lineRenderer.SetPosition(0, startPosition);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = 0;
            lineRenderer.SetPosition(1, mousePosition);
        }
    }

    // Ustawianie entity jako punkt Release
    public void SetReleaseEntity(int newReleaseEN)
    {
        Entity entity = editorNet.entityArray[newReleaseEN].GetComponent<Entity>();

        entity.type = 0;
        entity.color = 0;
        entity.action = 4;
        entity.linkedCatchEN = entityMenuScript.catchEntity.entityNumber;
        entityMenuScript.catchEntity.linkedReleaseEN = newReleaseEN;
        entity.ChangeColor();
        entity.ChangeTypeIcon();
        entity.ChangeActionIcon();
        entityMenuScript.isSettingRelease = false;
        lineRenderer.enabled = false;
    }
}
