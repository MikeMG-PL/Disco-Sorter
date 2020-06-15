using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_DissolveAmount", 0);
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        float x = 0;
        while (true)
        {
            mat.SetFloat("_DissolveAmount", Mathf.Sin(x));

            if (mat.GetFloat("_DissolveAmount") >= 0.86f)
            {
                Destroy(gameObject);
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
            }

            x += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

    }
}