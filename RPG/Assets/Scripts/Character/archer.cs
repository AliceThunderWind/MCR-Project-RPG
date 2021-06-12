using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcherState
{
    WalkRandom,
    Chase,
    BowAttack,
    BackToPos,
    NoAction
}

public class Archer : Enemy
{


    private ArcherState currentState;

    private bool attackOnCooldown = false;
    private Vector2 targetPosition;
    private float targetDirection;

    [SerializeField] private float attackCooldown;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = ArcherState.WalkRandom;


        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHit || CharacterState == CharacterState.Dead) return;
        mediator.ArcherBehaviour(this);
        switch (currentState)
        {
            case ArcherState.Chase:
                nextStep = vectorFromAngle(targetDirection);
                MoveCharacter(speed);
                break;
            case ArcherState.BackToPos:
                nextStep = vectorFromAngle(targetDirection);
                MoveCharacter(speed);
                break;
            case ArcherState.BowAttack:
                if(!attackOnCooldown)
                {
                    attackOnCooldown = true;
                    Attack();
                }
                else
                {
                    animator.SetBool("moving", false);
                }
                break;
            case ArcherState.WalkRandom:
                randomWalk();
                break;
            case ArcherState.NoAction:
                animator.SetBool("moving", false);
                break;
        }

    }

    private void Attack()
    {
        animator.SetFloat("targetX", targetPosition.x - transform.position.x);
        animator.SetFloat("targetY", targetPosition.y - transform.position.y);
        animator.GetBehaviour<archerAttack>().target = targetPosition;
        animator.GetBehaviour<archerAttack>().source = this;
        animator.SetTrigger("attackAvailable");
    }


    public void startAttackCooldown()
    {
        StartCoroutine(attackCooldownCo());
    }

    private IEnumerator attackCooldownCo()
    {
        yield return new WaitForSeconds(attackCooldown);
        endCooldown();
    }

    private void endCooldown()
    {
        attackOnCooldown = false;
    }

    public void setState(ArcherState state, float direction)
    {
        currentState = state;
        targetDirection = direction;
    }

    public void setTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }

    //Move these in Enemy

}


