using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBlink : MonoBehaviour
{
    bool isSet;
    public Material blinkMaterial;

    void Start()
    {

    }

    private void Update()
    {
        if (!isSet)
        {
            GetComponent<MeshRenderer>().material = blinkMaterial;
            blinkMaterial.SetFloat("_AlphaPower", 100);
            isSet = true;
        }
    }

    public IEnumerator Enable()
    {
        blinkMaterial.SetFloat("_AlphaPower", 3);

        yield return new WaitForSeconds(0.25f);

        blinkMaterial.SetFloat("_AlphaPower", 100);
    }
}