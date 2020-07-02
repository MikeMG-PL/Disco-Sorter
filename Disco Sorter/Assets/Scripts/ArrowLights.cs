using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLights : MonoBehaviour
{
    public List<Material> materials;
    public Material mat;
    [HideInInspector()]
    public Material bloom;
    bool bloomRunning, colorRunning; public bool topYellow;
    [HideInInspector()]
    public bool myLight;
    public new ArrowManager.Light light;
    [HideInInspector()]
    public ArrowManager.Light baseLight;
    [HideInInspector()]
    public float blinkSpeed;
    [HideInInspector()]
    public Color blinkColor;

    private void Start()
    {
        bloom = GetComponent<MeshRenderer>().material;
        NoColor();
        bloom.SetFloat("_AlphaPower", 30);
        baseLight = light;
    }

    public void NoColor()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(0.15f, 0.15f, 0.15f, 1));
        mat.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f, 1));
    }

    public void Blink(Color c, float s)
    {
        if (bloomRunning == false && colorRunning == false && myLight == true)
        {
            StartCoroutine(fixedBlinkBloom(c, s));
            StartCoroutine(fixedBlinkColor(c, s));
        }
    }

    private void Update()
    {
        Blink(blinkColor, blinkSpeed);
    }

    public IEnumerator fixedBlinkBloom(Color c, float s)
    {
        bloomRunning = true;
        float i = 0;
        bloom.SetFloat("_AlphaPower", 10);

        float multiplier;
        multiplier = s;
        if (s >= 15)
            multiplier = 10;

        for (i = 0; i < 1; i += Mathf.Sin(Time.fixedDeltaTime) * 10 * multiplier)
        {
            bloom.SetFloat("_AlphaPower", ClampToAlpha(i));
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(2 / s);
        i = 1; 

        

        for (i = 1; i > 0; i -= Mathf.Sin(Time.fixedDeltaTime) * 10 * multiplier)
        {
            bloom.SetFloat("_AlphaPower", ClampToAlpha(i));
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        i = 0;

        NoColor();
        bloom.SetFloat("_AlphaPower", 30);
        yield return new WaitForSeconds(2 / s);
        bloomRunning = false;
    }

    public IEnumerator fixedBlinkColor(Color c, float s)
    {
        colorRunning = true;
        float i = 1;

        mat.SetColor("_EmissionColor", c * ClampToIntensity(i));
        mat.SetColor("_Color", c);
        yield return new WaitForSeconds(2 / s);

        i = 0;

        mat.SetColor("_EmissionColor", c * ClampToIntensity(i));
        mat.SetColor("_Color", c);
        yield return new WaitForSeconds(Time.fixedDeltaTime);

        NoColor();
        yield return new WaitForSeconds(2 / s);
        colorRunning = false;
    }

    public float ClampToIntensity(float y)
    {
        return y * 1.5f;
    }

    public float ClampToAlpha(float y)
    {
        return 30 - y * 20;
    }
}
