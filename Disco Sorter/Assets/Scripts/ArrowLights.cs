using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLights : MonoBehaviour
{
    public Material mat;
    public Material bloom;
    public Material yellowMat;
    public Material yellowBloom;
    Material matBuffer, bloomBuffer;
    public bool topYellow;

    private void Start()
    {
        bloom = GetComponent<MeshRenderer>().material;
        NoColor();
        bloom.SetFloat("_AlphaPower", 100);
    }

    public void NoColor()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(0.15f, 0.15f, 0.15f, 1));
        mat.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f, 1));
        bloom.SetFloat("_AlphaPower", 100);
    }

    public IEnumerator fixedBlinkBloom(ArrowManager.Light l, ArrowManager.Hand hand, float ttb)
    {
        if (l == ArrowManager.Light.Yellow && topYellow)
        {
            Debug.Log("yellow!");
            bloomBuffer = bloom;
            bloom = yellowBloom;
        }

        GetComponent<MeshRenderer>().material = bloom;
        for (float i = 0; i < 1; i += Mathf.Sin(Time.fixedDeltaTime) * 10)
        {
            bloom.SetFloat("_AlphaPower", ClampToAlpha(i));
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(ttb);

        bloom.SetFloat("_AlphaPower", 5);

        yield return new WaitForSeconds(0.2f);

        NoColor();
        bloom.SetFloat("_AlphaPower", 100);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3.MENU")
            yield return new WaitForSeconds(1);

        if (l == ArrowManager.Light.Yellow && topYellow)
            bloom = bloomBuffer;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "3.MENU")
        {
            switch (hand)
            {
                case ArrowManager.Hand.Left:
                    transform.parent.GetComponentInParent<ArrowManager>().isDoneLeft = false;
                    break;
                case ArrowManager.Hand.Right:
                    transform.parent.GetComponentInParent<ArrowManager>().isDoneRight = false;
                    break;
            }
        }

    }

    public IEnumerator fixedBlinkColor(ArrowManager.Light l, ArrowManager.Hand hand, float ttb)
    {
        matBuffer = mat;
        Color c;

        switch (l)
        {
            case ArrowManager.Light.Red:
                c = Color.red;
                break;

            case ArrowManager.Light.Green:
                c = Color.green;
                break;

            case ArrowManager.Light.Yellow:
                c = Color.yellow;
                mat = yellowMat;
                break;

            default:
                c = Color.red;
                break;
        }

        transform.parent.GetComponent<MeshRenderer>().material = mat;
        mat.SetColor("_EmissionColor", c * 0.1f);
        mat.SetColor("_Color", c);
        yield return new WaitForSeconds(0.11f + ttb);
        mat.SetColor("_EmissionColor", c * 5);
        mat.SetColor("_Color", c);
        yield return new WaitForSeconds(0.2f);
        NoColor();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3.MENU")
            yield return new WaitForSeconds(1);

        if (mat != null)
            mat = matBuffer;
    }

    public float ClampToIntensity(float y)
    {
        return y * 1.5f;
    }

    public float ClampToAlpha(float y)
    {
        return 30 - y * 10;
    }
}
