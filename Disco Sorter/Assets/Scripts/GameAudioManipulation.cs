using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManipulation : MonoBehaviour
{
    [HideInInspector()]
    public AudioSource aSrc;

    void Awake()
    {
        aSrc = GetComponent<AudioSource>();
        aSrc.time = 0f;
    }
}
