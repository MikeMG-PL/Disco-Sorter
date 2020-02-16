using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameters : MonoBehaviour
{
    public float actionTime;
    public float actionStartTime;
    public float actionEndTime;
    public float linkedReleaseTime;                // Opcjonalnie, jeśli akcja to Catch... release (catch)
    public float linkedCatchTime;                  // Opcjonalnie, jeśli akcja to Catch... release (release)
    public float spawnTime;

    public int type;
    public int color;
    public int action;
    public int ID;
}
