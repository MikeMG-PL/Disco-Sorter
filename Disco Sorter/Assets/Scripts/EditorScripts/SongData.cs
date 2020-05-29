using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public List<EntityType> entityType = new List<EntityType>();
    public List<EntityColour> color = new List<EntityColour>();
    public List<EntityAction> action = new List<EntityAction>();
    public List<int> linkedReleaseEN = new List<int>();
    public List<int> linkedCatchEN = new List<int>();
    public int BPM;
    public int netDensity;
    public float clipLength;

    public SongData(EditorNet editorNet)
    {
        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            entityType.Add(entity.type);
            color.Add(entity.color);
            action.Add(entity.action);
            linkedReleaseEN.Add(entity.linkedReleaseEN);
            linkedCatchEN.Add(entity.linkedCatchEN);

        }
        BPM = editorNet.BPM;
        netDensity = editorNet.netDensity;
        clipLength = editorNet.gameObject.GetComponent<AudioManipulation>().floatClipLength;
    }
}
