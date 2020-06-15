using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseIcon : MonoBehaviour
{
    public Sprite green, red;
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

    public void LeftHand()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = true;
    }

    public void RightHand()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = false;
    }

    public void Green()
    { 
        spriteRenderer.sprite = green;
    }

    public void Red()
    {
        spriteRenderer.sprite = red;
    }
}
