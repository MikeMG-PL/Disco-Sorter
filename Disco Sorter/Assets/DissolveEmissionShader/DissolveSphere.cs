using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{
    Material mat; float timer;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_DissolveAmount", 0);
    }

    private void Update()
    {
        Dissolve();
    }

    void Dissolve()
    {
        timer += Time.deltaTime;
        mat.SetFloat("_DissolveAmount", Mathf.Sin(timer));

        if (mat.GetFloat("_DissolveAmount") >= 0.86f)
        {
            Destroy(gameObject);
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
        }
    }
}