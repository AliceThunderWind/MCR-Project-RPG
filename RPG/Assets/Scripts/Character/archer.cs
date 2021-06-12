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
       
        
       
        launchedAttack = true;
        while (currentState == EnemyState.Attack)
        {
            Vector3 target = mediator.PlayerPosition - transform.position;
            target.Normalize();
            animator.SetBool("moving", false);
            animator.SetFloat("targetX", target.x);
            animator.SetFloat("targetY", target.y);
            animator.GetBehaviour<archerAttack>().target = mediator.PlayerPosition;
            animator.GetBehaviour<archerAttack>().source = this;
            animator.SetTrigger("attackAvailable");
            animator.SetFloat("moveX", target.x);
            animator.SetFloat("moveY", target.y);
            yield return base.AttackCo();
            animator.SetBool("moving", true);
        }
        launchedAttack = false;
       
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


