using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public enum LaserColor { blue, green, pink, red, yellow, none }
    public enum Dir { left, right }

    public Dir direction;
    [HideInInspector()]
    public LaserColor col, prev;

    public List<GameObject> lasers;
    public List<Material> colors;

    [Header("Rotation")]
    public float rotationSpeed = 10f;
    public float rotateTime = 1.5f;
    [HideInInspector()]
    public float timer;
    Vector3 rot = new Vector3(0, 0, 0);

    void Awake()
    {
        col = (LaserColor)Random.Range(0, colors.Count);
        prev = col;
    }

    void Update()
    {
        RandomColor();
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

    void RandomColor()
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            ChangeColor(col, lasers[i]);
        }
    }

    void Direction()
    {
        if (direction == Dir.right)
            rot.y += rotationSpeed * Time.deltaTime;
        else if (direction == Dir.left)
            rot.y -= rotationSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        rot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        Direction();
        transform.localEulerAngles = rot;


        timer += Time.deltaTime;

        if (timer >= rotateTime && rotateTime != 0)
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
