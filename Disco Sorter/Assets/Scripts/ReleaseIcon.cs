using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseIcon : MonoBehaviour
{
    public List<Sprite> sprites;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(10, 0, 0);
    }

    public void LeftRed()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprites[0];
    }

    public void RightRed()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprites[1];
    }

    public void LeftGreen()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprites[2];
    }

    public void RightGreen()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprites[3];
    }

    public void Rotten()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprites[4];
    }
}
