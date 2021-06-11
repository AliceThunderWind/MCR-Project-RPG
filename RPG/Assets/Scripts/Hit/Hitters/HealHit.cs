using Assets.Scripts.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealHit : MonoBehaviour
{

    [SerializeField] private float healAmount;
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

        Player who = collision.GetComponent<Player>();
        if(who != null)
        {
            HpIncreaseCommand cmd = new HpIncreaseCommand();
            cmd.What = collision;
            cmd.Hp = healAmount;
            mediator.Publish(cmd);
        }

    }
}
