using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseIcon : MonoBehaviour
{
    public Sprite green, red;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(10, 0, 0);
    }

    public void LeftHand()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = true;
    }

    public void RightHand()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = false;
    }

    public void Green()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = green;
    }

    public void Red()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = red;
    }
}
