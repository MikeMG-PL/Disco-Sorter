using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMethods : MonoBehaviour
{
    public GameObject discoFractured;
    public Material dissolveMaterial;
    public Material mainMaterial;
    public bool dissolve;
    bool dissolving, isChecked, performing, thisIsMenu;
    GameObject g;
    Transform distanceCounter;
    PointManager pointManager;

    void Awake()
    {
        if (!transform.CompareTag("DiscoBall"))
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = mainMaterial;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "3.MENU") thisIsMenu = true;
        else pointManager = GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>();

        dissolveMaterial.SetFloat("_DissolveAmount", 0);
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;

        dissolve = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
            dissolve = true;
    }

    void Update()
    {
        if (!transform.CompareTag("DiscoBall") && !dissolve)
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = mainMaterial;

        if (GetComponent<ObjectParameters>().wasGrabbed)
            dissolve = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        g = collision.gameObject;

        // punktowanie niewrzucenia jabłka do skrzyni
        if ((g.CompareTag("Plane") || g.CompareTag("Building")) && !gameObject.CompareTag("Release") &&
            !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer &&
            !isChecked && GetComponent<ObjectParameters>().wasGrabbed)
        {
            isChecked = true;

            if (gameObject.CompareTag("RottenApple"))
            {
                distanceCounter = GameObject.FindGameObjectWithTag("DistanceCounter").transform;
                pointManager.ThrowPoints(PointManager.AppleState.RottenThrow, Vector3.Distance(distanceCounter.transform.position, transform.position));
            }

            else
                pointManager.ThrowPoints(PointManager.AppleState.NoBox, 0);
        }



        if (g.CompareTag("Building") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && dissolve)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());

        }
        if (g.CompareTag("Building") && gameObject.CompareTag("DiscoBall"))
        {
            DestroyDisco();
        }
        if (g.CompareTag("Plane") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            StartCoroutine(Dissolve());

        }
    }

    public void DestroyDisco()
    {
        GameObject f = Instantiate(discoFractured, transform.position, Quaternion.identity);
        f.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        // punktowanie niewrzucenia jabłka do skrzyni
        if ((g.CompareTag("Plane") || g.CompareTag("Building")) && !gameObject.CompareTag("Release") &&
            !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer
            && !isChecked && GetComponent<ObjectParameters>().wasGrabbed)
        {
            isChecked = true;

            if (gameObject.CompareTag("RottenApple"))
            {
                distanceCounter = GameObject.FindGameObjectWithTag("DistanceCounter").transform;
                pointManager.ThrowPoints(PointManager.AppleState.RottenThrow, Vector3.Distance(distanceCounter.transform.position, transform.position));
            }
            else
                pointManager.ThrowPoints(PointManager.AppleState.NoBox, 0);
        }



        if (g.CompareTag("Plane") && !gameObject.CompareTag("Release") && !gameObject.CompareTag("DiscoBall") && gameObject.GetComponent<ObjectParameters>().linkedReleaseTimeEnd < LevelManager.timer)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = dissolveMaterial;
            if (!performing)
            {
                StartCoroutine(Dissolve());
                performing = true;
            }
        }
    }

    public IEnumerator Dissolve()
    {
        switch (GetComponent<ObjectParameters>().color)
        {
            case EntityColour.Red:
                MainMenuManager.reds--;
                break;

            case EntityColour.Green:
                MainMenuManager.greens--;
                break;

            case EntityColour.None:
                MainMenuManager.yellows--;
                break;
        }

        if (!dissolving)
        {
            float x = 0;
            dissolving = true;
            while (true)
            {
                dissolveMaterial.SetFloat("_DissolveAmount", Mathf.Sin(x) * 2);

                if (dissolveMaterial.GetFloat("_DissolveAmount") >= 0.86f && (thisIsMenu || !GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>().levelFailed))
                {
                    Destroy(gameObject);
                    if (transform.parent != null && (transform.parent.CompareTag("Apple") || transform.CompareTag("RottenApple")))
                    {
                        Destroy(transform.parent.gameObject);
                        dissolving = false;
                        StopCoroutine(Dissolve());
                    }
                }

                x += Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }
    }

    public IEnumerator RevertedDissolve()
    {
        float x = 90;

        while (Mathf.Sin(x) >= 0)
        {
            dissolveMaterial.SetFloat("_DissolveAmount", Mathf.Sin(x));

            x -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            if (x <= 0.05 && GetComponent<ObjectParameters>().type != EntityType.Disco)
            {
                switch (GetComponent<ObjectParameters>().color)
                {
                    case EntityColour.Red:
                        MainMenuManager.reds++;
                        break;

                    case EntityColour.Green:
                        MainMenuManager.greens++;
                        break;

                    case EntityColour.None:
                        MainMenuManager.yellows++;
                        break;
                }
            }
            x = -0.1f;
            StopCoroutine(RevertedDissolve());
        }
    }
}
