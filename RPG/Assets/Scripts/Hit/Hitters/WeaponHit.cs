using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    [SerializeField] private float damage;
    private CommandDispatcher mediator = CommandDispatcher.Instance;

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

            HitCharacterCommand cmd = new HitCharacterCommand();
            cmd.What    = other;
            cmd.Damage  = damage;
            mediator.Publish(cmd);

        }else if(other.CompareTag("brakeable")) {
            HitBreakeableCommand cmd = new HitBreakeableCommand();
            cmd.What = other;
            mediator.Publish(cmd);
        }
    }
}
