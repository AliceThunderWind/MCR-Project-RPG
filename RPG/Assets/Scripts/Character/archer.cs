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


    private bool attackOnCooldown = false;

    [SerializeField] private float attackCooldown;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = EnemyState.WalkRandom;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);
    }

    // Update is called once per frame
   

    protected override IEnumerator AttackCo()
    {
        animator.SetFloat("targetX", mediator.PlayerPosition.x - transform.position.x);
        animator.SetFloat("targetY", mediator.PlayerPosition.x - transform.position.y);
        animator.GetBehaviour<archerAttack>().target = mediator.PlayerPosition;
        animator.GetBehaviour<archerAttack>().source = this;
        animator.SetTrigger("attackAvailable");
        return base.AttackCo();
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


