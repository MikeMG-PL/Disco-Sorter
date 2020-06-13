using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMethods : MonoBehaviour
{
    public GameObject discoFractured;
    public Material dissolveMaterial;
    float timer;

    void Start()
    {
        dissolveMaterial.SetFloat("_DissolveAmount", 0);

        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall"))
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            Dissolve();
        }
        else if(collision.gameObject.CompareTag("Building") && !gameObject.CompareTag("Release") && gameObject.CompareTag("DiscoBall"))
        {
            Instantiate(discoFractured, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Dissolve()
    {
        timer += Time.deltaTime;
        dissolveMaterial.SetFloat("_DissolveAmount", Mathf.Sin(timer));

        if (dissolveMaterial.GetFloat("_DissolveAmount") >= 0.86f)
        {
            Destroy(gameObject);
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
        }
    }
}
