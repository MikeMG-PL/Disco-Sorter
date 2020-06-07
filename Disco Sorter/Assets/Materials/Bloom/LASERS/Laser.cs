using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [HideInInspector()]
    public enum LaserColor { blue, green, pink, red, yellow }
    public enum Dir { left, right }
    Dir direction;
    [HideInInspector()]
    public LaserColor col;

    public GameObject laser1, laser2, laser3;
    [Header("Colors")]
    public List<Material> colors;

    [Header("Rotation")]
    public float rotationSpeed = 10f;
    public float rotateTime = 1.5f;
    float timer;
    Vector3 rot = new Vector3(0, 0, 0);

    void Start()
    {
        direction = Dir.right;
        col = (LaserColor)Random.Range(0, 5);
    }

    void Update()
    {
        ChangeColor(col, laser1);
        ChangeColor(col, laser2);
        ChangeColor(col, laser3);
        Rotate();
    }

    void ChangeColor(LaserColor c, GameObject laser)
    {
        switch (c)
        {
            case LaserColor.blue:
                laser.GetComponent<MeshRenderer>().material = colors[(int)LaserColor.blue];
                break;
            case LaserColor.green:
                laser.GetComponent<MeshRenderer>().material = colors[(int)LaserColor.green];
                break;
            case LaserColor.pink:
                laser.GetComponent<MeshRenderer>().material = colors[(int)LaserColor.pink];
                break;
            case LaserColor.red:
                laser.GetComponent<MeshRenderer>().material = colors[(int)LaserColor.red];
                break;
            case LaserColor.yellow:
                laser.GetComponent<MeshRenderer>().material = colors[(int)LaserColor.yellow];
                break;
        }
    }

    void Direction()
    {
        if(direction == Dir.right)
            rot.y += rotationSpeed * Time.fixedDeltaTime;
        else if(direction == Dir.left)
            rot.y -= rotationSpeed * Time.fixedDeltaTime;
    }

    void Rotate()
    {
        rot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        Direction();
        transform.localEulerAngles = rot;

        
        timer += Time.fixedDeltaTime;

        if(timer >= rotateTime)
        {
            switch (direction)
            {
                case Dir.right:
                    direction = Dir.left;
                    break;

                case Dir.left:
                    direction = Dir.right;
                    break;
            }
            timer = 0;
        }
    }
}
