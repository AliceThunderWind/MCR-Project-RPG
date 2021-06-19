using System.Collections;
using UnityEngine;

public class Archer : Enemy
{

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
            animator.SetBool("moving", false);
            animator.SetFloat("targetX", vectorToTarget.x);
            animator.SetFloat("targetY", vectorToTarget.y);
            animator.GetBehaviour<ArcherAttack>().target = mediator.getPlayer();
            animator.GetBehaviour<ArcherAttack>().source = this;
            animator.SetTrigger("attackAvailable");
            animator.SetFloat("moveX", vectorToTarget.x);
            animator.SetFloat("moveY", vectorToTarget.y);
            yield return base.AttackCo();
            animator.SetBool("moving", true);
        }
        launchedAttack = false;
       
    }



    //Move these in Enemy

}


