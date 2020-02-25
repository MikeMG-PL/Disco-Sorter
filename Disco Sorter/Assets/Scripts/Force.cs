using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    float timer;
    public Transform secondBall;
    public Transform thirdBall;
    float height;
    float length;
    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;

        if (secondBall != null)
            height = Vector3.Distance(gameObject.transform.position, secondBall.position);

        if (secondBall != null)
            length = Vector3.Distance(gameObject.transform.position, thirdBall.position);

        Debug.Log("Height: " + height);
        Debug.Log("Length: " + length);
    }
    private void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.001)
            timer += Time.deltaTime;
        //Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
            Debug.Log(timer);
    }
}
