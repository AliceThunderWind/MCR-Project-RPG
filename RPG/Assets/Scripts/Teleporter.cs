using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Transform target; // la cible qui peut utiliser le teleporter
    //public Vector2 cameraChange;
    private CameraMovement cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == target.tag)
        {
            //cam.minPosition += cameraChange;
            //cam.maxPosition += cameraChange;
            other.transform.position = transform.GetChild(0).position;
        }
    }
}
