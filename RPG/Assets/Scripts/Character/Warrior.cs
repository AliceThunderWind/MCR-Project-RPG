using Assets.Scripts.Mediator;
using System;
using System.Collections;
using UnityEngine;

public enum WarriorState
{
    WalkRandom,
    Chase,
    MeleeAttack,
    BackToPos,
    NoAction
}
public class Warrior : Enemy
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = EnemyState.WalkRandom;

        nextStopTime = nextStartTime + period;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);

        vectorToTarget = GameMediator.RandomVector();
    }


    protected override IEnumerator AttackCo()
    {
        launchedAttack = true;
        animator.SetBool("moving", false);
        while (currentState == EnemyState.Attack)
        {
            yield return base.AttackCo();
        }
        launchedAttack = false;
    }


}
