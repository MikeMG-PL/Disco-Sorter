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
    public void SetReleaseEntity(Entity newReleaseEntity)
    {
        newReleaseEntity.type = EntityType.None;
        newReleaseEntity.color = EntityColour.None;
        newReleaseEntity.action = EntityAction.ReleasePoint;
        newReleaseEntity.linkedCatchEN = entityMenuScript.catchEntity.entityNumber;
        entityMenuScript.catchEntity.linkedReleaseEN = newReleaseEntity.entityNumber;
        newReleaseEntity.ChangeColor();
        newReleaseEntity.ChangeTypeIcon();
        newReleaseEntity.ChangeActionIcon();
        entityMenuScript.isSettingRelease = false;
        lineRenderer.enabled = false;
    }

    public void SetUnreleaseEntity(Entity entityToUnrelease)
    {
        Entity linkedCatchEntity = editorNet.entityArray[entityToUnrelease.linkedCatchEN].GetComponent<Entity>();
        linkedCatchEntity.action = EntityAction.None;
        linkedCatchEntity.linkedReleaseEN = -1;

        entityToUnrelease.action = EntityAction.None;
        entityToUnrelease.linkedCatchEN = -1;
        entityToUnrelease.ChangeActionIcon();
    }
}
