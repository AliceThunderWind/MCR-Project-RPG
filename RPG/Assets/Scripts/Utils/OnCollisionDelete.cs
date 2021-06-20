using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant le comportement afin de détruire un objet lors d'une collision
/// </summary>
public class OnCollisionDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
