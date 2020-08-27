using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseIcon : MonoBehaviour
{
    public List<Sprite> sprites; float alpha, childAlphaPower; bool disabling;
    SpriteRenderer spriteRenderer; MeshRenderer childRenderer;
    [HideInInspector()]
    public bool disabled;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        childRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        spriteRenderer.enabled = false;
        childRenderer.enabled = false;
        Activate();
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(10, 0, 0);
    }

    public IEnumerator Enable()
    {
        /*if (disabling)
            StopCoroutine(Enable());
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = 0;
        spriteRenderer.enabled = true;

        while (alpha < 1 && spriteRenderer != null)
        {
            alpha += Time.fixedDeltaTime * 2;
            spriteRenderer.material.color = new Color(spriteRenderer.material.color.r, spriteRenderer.material.color.g, spriteRenderer.material.color.b, alpha);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        StopCoroutine(Enable());*/
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Disable()
    {
        disabling = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        while (alpha > 0 && spriteRenderer != null)
        {
            alpha -= Time.fixedDeltaTime * 6;
            spriteRenderer.material.color = new Color(spriteRenderer.material.color.r, spriteRenderer.material.color.g, spriteRenderer.material.color.b, alpha);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        disabling = false;

        if (spriteRenderer != null)
        {
            disabled = true;
            Destroy(gameObject);
            StopCoroutine(Disable());
        }
        
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator EnableFog()
    {
        /*if (disabling)
            StopCoroutine(EnableFog());
        childRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        childAlphaPower = 15;
        childRenderer.enabled = true;

        while (childAlphaPower > 3f && childRenderer != null)
        {
            childAlphaPower -= Time.fixedDeltaTime * 30;
            childRenderer.material.SetFloat("_AlphaPower", childAlphaPower);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        StopCoroutine(EnableFog());*/
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator DisableFog()
    {
        disabling = true;
        childRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        while (childAlphaPower < 50 && childRenderer != null)
        {
            childAlphaPower += Time.fixedDeltaTime * 90;
            childRenderer.material.SetFloat("_AlphaPower", childAlphaPower);
            childRenderer.material.SetFloat("_Color", alpha);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        disabling = false;

        if (childRenderer != null)
        {
            Destroy(gameObject);
            StopCoroutine(DisableFog());
        }
    }

    public void Activate()
    {
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        //StartCoroutine(Enable());
        //StartCoroutine(EnableFog());
    }

    public void LeftRed()
    {
        Activate();
        spriteRenderer.sprite = sprites[0];
    }

    public void RightRed()
    {
        Activate();
        spriteRenderer.sprite = sprites[1];
    }

    public void LeftGreen()
    {
        Activate();
        spriteRenderer.sprite = sprites[2];
    }

    public void RightGreen()
    {
        Activate();
        spriteRenderer.sprite = sprites[3];
    }

    public void Rotten()
    {
        Activate();
        spriteRenderer.sprite = sprites[4];
    }
}
