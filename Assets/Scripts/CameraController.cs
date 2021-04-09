using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 0.3f;
    public GameObject player;
    public float minRotX = -2;
    public float maxRotX = 2;
    public float minRotY;
    public float maxRotY;

    // Start is called before the first frame update
    void Start()
    {
        // Follow the player
        player = GameObject.Find("TheThing");
        transform.localPosition = new Vector3(0, 4, -10);
    }
}
    