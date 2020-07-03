using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLights : MonoBehaviour
{
    public List<Material> materials;
    public Material mat;
    [HideInInspector()]
    public Material bloom;
    [HideInInspector()]
    public bool bloomRunning, colorRunning;
    public bool topYellow;
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
        bloom.SetFloat("_AlphaPower", 100);
        baseLight = light;
    }

    public void NoColor()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(0.15f, 0.15f, 0.15f, 1));
        mat.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f, 1));
        bloom.SetFloat("_AlphaPower", 100);
    }

    public void Blink(Color c, float s)
    {
        if (bloomRunning == false && colorRunning == false && myLight == true)
        {
            StartCoroutine(fixedBlinkBloom(c));
            StartCoroutine(fixedBlinkColor(c));
        }
    }

    private void Update()
    {
        Blink(blinkColor, blinkSpeed);
    }

    public IEnumerator fixedBlinkBloom(Color c)
    {
        bloomRunning = true;
        for (float i = 0; i < 1; i += Mathf.Sin(Time.fixedDeltaTime) * 10)
        {
            bloom.SetFloat("_AlphaPower", ClampToAlpha(i));
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(1);

        bloom.SetFloat("_AlphaPower", 8);

        yield return new WaitForSeconds(0.2f);

        NoColor();
        bloom.SetFloat("_AlphaPower", 100);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3.MENU")
            yield return new WaitForSeconds(1);

        bloomRunning = false;
    }

    public IEnumerator fixedBlinkColor(Color c)
    {
        colorRunning = true;

        mat.SetColor("_EmissionColor", c * 3);
        mat.SetColor("_Color", c);
        yield return new WaitForSeconds(1.31f);
        NoColor();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3.MENU")
            yield return new WaitForSeconds(1);

        colorRunning = false;
    }

    public float ClampToIntensity(float y)
    {
        return y * 1.5f;
    }

    public float ClampToAlpha(float y)
    {
        return 30 - y * 15;
    }
}
