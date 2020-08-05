using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDiscoLevel : MonoBehaviour
{
    void OnDestroy()
    {
        transform.parent.GetComponent<ChooseLevel>().LaunchLevel();
    }
}
