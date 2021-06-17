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

            Character target = mediator.FindClosestEnemy(this);
            animator.SetBool("attacking", true);
            animator.GetBehaviour<ArcherAttack>().target = target;
            animator.GetBehaviour<ArcherAttack>().source = this;
            animator.SetTrigger("attackAvailable");
            animator.SetFloat("moveX", target.Position.x);
            animator.SetFloat("moveY", target.Position.y);
            yield return base.AttackCo();
            animator.SetBool("moving", true);

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
