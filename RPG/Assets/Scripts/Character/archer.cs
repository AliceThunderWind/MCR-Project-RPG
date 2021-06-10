using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcherState
{
    WalkRandom,
    Chase,
    BowAttack
}

public class archer : Enemy
{


    private ArcherState currentState;

    private bool attackOnCooldown = false;
    private float distanceToPlayer;
    private float directionToPlayer;

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

        nextStep = RandomVector();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit || characterState == CharacterState.Dead) return;

        distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
        directionToPlayer = direction(transform.position, playerPosition);

        if (distanceToPlayer < fightDistance)
        {
            currentState = ArcherState.BowAttack;
            animator.SetBool("moving", false);
            if (!attackOnCooldown)
            {
                attackOnCooldown = true;
                testAttack();
            }
        }
        else if (distanceToPlayer < visibility)
        {
            currentState = ArcherState.Chase;
            nextStep = vectorFromAngle(directionToPlayer);
            MoveCharacter(speed);
        }
        else
        {
            currentState = ArcherState.WalkRandom;
            randomWalk();
        }

    }

    private void testAttack()
    {
        animator.SetBool("attackAvailable", true);
        animator.SetFloat("targetX", playerPosition.x - this.transform.position.x);
        animator.SetFloat("targetY", playerPosition.y - this.transform.position.y);
        animator.GetBehaviour<archerAttack>().target = playerPosition;
        animator.GetBehaviour<archerAttack>().source = this;
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

    //Move these in Enemy

}
