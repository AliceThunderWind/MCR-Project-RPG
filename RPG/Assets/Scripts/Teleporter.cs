using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Transform target; // la cible qui peut utiliser le teleporter
    private CameraMovement cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

            other.transform.position = transform.GetChild(0).position;

    }
}
