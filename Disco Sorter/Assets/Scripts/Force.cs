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
        timer = 0;
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 10000f;
    }
    private void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.001)
            timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Finish")
            Debug.Log(timer);
    }
}
