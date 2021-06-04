using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{

    private Mediator mediator = Mediator.Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /* 
         * Limit Hit triggers only on Collider elements with attackable tag
         * Will not Publish HitCommand in case he hits not attackable Collider 
         * object such as Rocks
         * Each object that can be attacked by the player will have to use
         * attackable tag
         */
        if (other.CompareTag("attackable")) { 
            PlayerHitCommand cmd = new PlayerHitCommand();
            cmd.What = other;
            mediator.Publish(cmd);
        }
    }
}
