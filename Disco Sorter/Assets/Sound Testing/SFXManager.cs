using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip onTime;
    public AudioClip correctBox;
    public AudioClip wrong;

    public List<AudioClip> customClips;

    public void PlaySound(AudioClip c)
    {
        GetComponent<AudioSource>().clip = c;
        GetComponent<AudioSource>().Play();
    }
}
