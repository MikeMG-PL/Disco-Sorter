using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public List<int> entityType = new List<int>();
    public List<int> color = new List<int>();
    public List<int> action = new List<int>();
    public List<int> linkedReleaseEN = new List<int>();
    public List<int> linkedCatchEN = new List<int>();
    public int BPM, netDensity;

    public SongData(EditorNet editorNet, LevelParameters levelParameters)
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

        for (int i = 0; i < editorNet.entityArray.Length; i++)
        {
            Entity entity = editorNet.entityArray[i].GetComponent<Entity>();
            levelParameters.entityType.Add(entity.type);
            levelParameters.color.Add(entity.color);
            levelParameters.action.Add(entity.action);
            levelParameters.linkedReleaseEN.Add(entity.linkedReleaseEN);
            levelParameters.linkedCatchEN.Add(entity.linkedCatchEN);
        }

        BPM = editorNet.BPM;
        netDensity = editorNet.netDensity;
    }
}
