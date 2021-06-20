using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant un joueur tirant à distance
/// </summary>
/// <inheritdoc/>
public class PlayerProjectile : Player
{
    protected override IEnumerator AttackCo()
    {
        if (CharacterState != CharacterState.Dead)
        {
            CharacterState = CharacterState.Attack;
            Character target = mediator.FindClosestEnemy(this);
            if(target != null) { 
                Vector3 targetVector = target.Position - Position;
                targetVector.Normalize();
                animator.SetBool("attacking", true);
                animator.GetBehaviour<ProjectileAttack>().target = target;
                animator.GetBehaviour<ProjectileAttack>().source = this;
                animator.SetFloat("targetX", targetVector.x);
                animator.SetFloat("targetY", targetVector.y);
                animator.SetTrigger("attackAvailable");
                animator.SetFloat("moveX", targetVector.x);
                animator.SetFloat("moveY", targetVector.y);
                yield return new WaitForSeconds(attackDuration);
                animator.SetBool("attacking", false);
                yield return new WaitForSeconds(attackCoolDown);
                animator.SetBool("moving", true);
            }
            else
            {
                yield return null;                
            }
            CharacterState = CharacterState.Idle;
        }
    }

}
