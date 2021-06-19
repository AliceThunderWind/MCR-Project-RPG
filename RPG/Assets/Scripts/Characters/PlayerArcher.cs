using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcher : Player
{

    protected override IEnumerator AttackCo()
    {

        if (CharacterState != CharacterState.Dead)
        {
            CharacterState = CharacterState.Attack;
            Character target = mediator.FindClosestEnemy(this);
            Vector3 targetVector = target.Position - Position;
            targetVector.Normalize();
            animator.SetBool("attacking", true);
            animator.GetBehaviour<ArcherAttack>().target = target;
            animator.GetBehaviour<ArcherAttack>().source = this;
            animator.SetFloat("targetX", targetVector.x);
            animator.SetFloat("targetY", targetVector.y);
            animator.SetTrigger("attackAvailable");
            animator.SetFloat("moveX", targetVector.x);
            animator.SetFloat("moveY", targetVector.y);
            yield return new WaitForSeconds(attackDuration);
            animator.SetBool("attacking", false);
            yield return new WaitForSeconds(attackCoolDown);
            animator.SetBool("moving", true);
            CharacterState = CharacterState.Idle;
        }
    }

    public override void Update()
    {
        if (!Selected) return;

        animator.SetBool("moving", false);
        vectorToTarget = Vector3.zero;
        vectorToTarget.x = Input.GetAxisRaw("Horizontal");
        vectorToTarget.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("SwordAttack") && CharacterState != CharacterState.Attack)
        {
            StartCoroutine(AttackCo());
        }

        if (vectorToTarget != Vector3.zero)
        {
            MoveCharacter(speed);
        }

    }
    

}
