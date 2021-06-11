using Assets.Scripts.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;
    private CommandDispatcher mediator = CommandDispatcher.Instance;

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
        Rigidbody2D other = collision.GetComponent<Rigidbody2D>();
        if(other != null)
        {
            Vector2 difference = new Vector3(other.position.x, other.position.y, 0) - transform.position;
            difference = difference.normalized * thrust;
            KnockbackCommand cmd = new KnockbackCommand();
            cmd.body = other;
            cmd.force = difference;
            mediator.Publish(cmd);
        }

    }

}
