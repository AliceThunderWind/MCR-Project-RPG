using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float thrust;
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
            if (thrust > 0f)
                knockBack(other);

            HitCharacterCommand cmd = new HitCharacterCommand();
            cmd.What    = other;
            cmd.Damage  = damage;
            mediator.Publish(cmd);

        }else if(other.CompareTag("brakeable")) {
            HitObjectCommand cmd = new HitObjectCommand();
            cmd.What = other;
            mediator.Publish(cmd);
        }
    }

    private void knockBack(Collider2D other)
    {
        Rigidbody2D victim = other.GetComponent<Rigidbody2D>();
        if(victim != null)
        {
            Vector2 difference = new Vector3(victim.position.x, victim.position.y, 0) - transform.position;
            difference = difference.normalized * thrust;
            victim.AddForce(difference, ForceMode2D.Impulse);
        }
    }
}
