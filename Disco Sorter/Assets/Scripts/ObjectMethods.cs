using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMethods : MonoBehaviour
{
    public GameObject discoFractured;
    public Material dissolveMaterial;
    float timer;
    public bool dissolve;
    GameObject g;

    void Start()
    {
        dissolveMaterial.SetFloat("_DissolveAmount", 0);
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }

    void OnCollisionEnter(Collision collision)
    {
        g = collision.gameObject;

        if (g.CompareTag("Building") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && dissolve)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());
        }
        else if(g.CompareTag("Building") && !gameObject.CompareTag("Release") && gameObject.CompareTag("DiscoBall"))
        {
            Instantiate(discoFractured, transform.position, Quaternion.identity);    
            Destroy(gameObject);
        }
        else if(g.CompareTag("Plane") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());
        }
    }

    IEnumerator Dissolve()
    {
        float x = 0;
        while (true)
        {
            dissolveMaterial.SetFloat("_DissolveAmount", Mathf.Sin(x) * 2);

            if (dissolveMaterial.GetFloat("_DissolveAmount") >= 0.86f)
            {
                Destroy(gameObject);
                if (transform.parent != null && (transform.parent.CompareTag("Apple") || transform.CompareTag("RottenApple")))
                    Destroy(transform.parent.gameObject);
            }

            x += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
