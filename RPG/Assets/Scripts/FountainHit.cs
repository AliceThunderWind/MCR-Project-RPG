using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainHit : MonoBehaviour
{
    private Mediator mediator = Mediator.Instance;

    private bool playerInRange;
    private int nextUpdate = 1; // delay update in seconds

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            HpIncreaseCommand cmd = new HpIncreaseCommand();
            cmd.Hp = 10f;
            mediator.Publish(cmd);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
    }
}
