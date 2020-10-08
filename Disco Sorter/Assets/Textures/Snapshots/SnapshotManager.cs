using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotManager : MonoBehaviour
{
    public Sprite hit, lights, throwApple; float timer;

    private void Awake()
    {
        timer = 0;
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = hit;   
    }

    void ChangeSprite(Sprite sprite)
    {
        if (GetComponent<SpriteRenderer>().sprite != sprite)
            GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Update()
    {

        timer += Time.fixedDeltaTime;

        if (timer > 0 && timer <= 8)
            ChangeSprite(hit);
        if (timer > 8 && timer <= 16)
            ChangeSprite(lights);
        if (timer > 16 && timer <= 24)
            ChangeSprite(throwApple);
        if (timer > 24)
            timer = 0;
    }


}
