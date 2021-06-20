using Assets.Scripts.Hit;
using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant un dommage de type confusion
/// </summary>
public class ConfuseHit : MonoBehaviour
{
    public GameMediator mediator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("attackable"))
        {
            ICharacter character = other.GetComponent<ICharacter>();
            if (character != null)
                character.getMediator().CharacterConfuse(character);
        }
    }
  
}
