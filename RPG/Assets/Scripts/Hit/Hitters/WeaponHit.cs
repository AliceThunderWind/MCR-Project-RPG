using Assets.Scripts.Hit;
using UnityEngine;

/// <summary>
/// Classe représentant un dommage standard
/// </summary>
public class WeaponHit : MonoBehaviour
{
    [SerializeField] private float damage;
    private CommandDispatcher commandDispatcher = CommandDispatcher.Instance;

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
            ICharacter character = other.GetComponent<ICharacter>();
            if(character != null)
                character.getMediator().CharachterHit(character, damage);

        }else if(other.CompareTag("brakeable")) {
            HitBreakeableCommand cmd = new HitBreakeableCommand();
            cmd.What = other;
            commandDispatcher.Publish(cmd);
        }
    }
}
