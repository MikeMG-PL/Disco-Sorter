using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoRotate : MonoBehaviour
{
    public float speed = 10;

    void Awake()
    {
        if (speed > 360)
            speed = 360;
    }

    void Update()
    {
        transform.localEulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
        if(transform.localEulerAngles.y >= 360)
            transform.localEulerAngles -= new Vector3(0, 360 * Time.deltaTime, 0);
    }
}
