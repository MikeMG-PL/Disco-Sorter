using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseIcon : MonoBehaviour
{
    public List<Sprite> sprites;
    SpriteRenderer spriteRenderer; MeshRenderer childRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        childRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        spriteRenderer.enabled = false;
        childRenderer.enabled = false;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(10, 0, 0);
    }

    public void LeftRed()
    {
        spriteRenderer.enabled = true;
        childRenderer.enabled = true;
        spriteRenderer.sprite = sprites[0];
    }

    public void RightRed()
    {
        spriteRenderer.enabled = true;
        childRenderer.enabled = true;
        spriteRenderer.sprite = sprites[1];
    }

    public void LeftGreen()
    {
        spriteRenderer.enabled = true;
        childRenderer.enabled = true;
        spriteRenderer.sprite = sprites[2];
    }

    public void RightGreen()
    {
        spriteRenderer.enabled = true;
        childRenderer.enabled = true;
        spriteRenderer.sprite = sprites[3];
    }

    public void Rotten()
    {
        spriteRenderer.enabled = true;
        childRenderer.enabled = true;
        spriteRenderer.sprite = sprites[4];
    }
}
