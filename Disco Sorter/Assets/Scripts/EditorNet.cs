using System;
using UnityEngine;

public class EditorNet : MonoBehaviour
{
    public GameObject entity;
    public AudioClip clip;
    //public Material activeMaterial;

    private Vector3 positionToSpawnEntity = Vector3.zero;
    private GameObject createdEntity;
    private GameObject[] entityArray;

    void Start()
    {
        entityArray = new GameObject[Convert.ToInt32(clip.length)];
        for (int i = 0; i < clip.length; i++)
        {
            createdEntity = Instantiate(entity, positionToSpawnEntity, Quaternion.identity);
            createdEntity.GetComponent<Entity>().EntityNumber = i;
            entityArray[i] = createdEntity;
            positionToSpawnEntity.x = positionToSpawnEntity.x + 1.05f;
        }
    }

    private double currentTime;
    private int entityNumber;
    void Update()
    {
        currentTime = gameObject.GetComponent<AudioSource>().time;
        entityNumber = Convert.ToInt32(currentTime);
        if (entityArray[0] != null)
            entityArray[entityNumber].GetComponent<Renderer>().material.color = Color.green;
    }
}
