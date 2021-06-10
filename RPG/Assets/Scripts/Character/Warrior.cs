using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

public enum WarriorState
{
    WalkRandom,
    Chase,
    SpearAttack,
    backToPos
}
public class Warrior : Enemy
{
    [SerializeField] private float chaseSpeed;
    [SerializeField] private bool confuse;

    private WarriorState currentState;



    private bool launchedAttack = false;
    private float distanceToTarget;
    private float directionToTarget;


    // for random walk in 5 second periode with 5s pause


    private static readonly System.Random getrandom = new System.Random(DateTime.Now.Millisecond);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = WarriorState.WalkRandom;

        nextStopTime = nextStartTime + period;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);

        nextStep = RandomVector();
        mediator.Subscribe<PlayerChangePositionCommand>(OnPlayerChangePosition);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time + " | act time : " + nextActionTime + " stop time : " + nextStopTime);
        if (isHit || characterState == CharacterState.Dead) return;


        if(confuse)
        {
            AskClosestEnemyCommand cmd = new AskClosestEnemyCommand();
            cmd.source = this;
            mediator.Publish(cmd);
            if (closestEnemy == null) return;
            distanceToTarget = Vector3.Distance(closestEnemy.transform.position, transform.position);
            directionToTarget = direction(transform.position, closestEnemy.transform.position);

        }
        else
        {
            distanceToTarget = Vector3.Distance(playerPosition, transform.position);
            directionToTarget = direction(transform.position, playerPosition);
        }


        if (distanceToTarget < fightDistance)
        {
            currentState = WarriorState.SpearAttack;
            if (!launchedAttack) StartCoroutine(AttackCo());
        }
        else if (distanceToTarget < visibility)
        {
            currentState = WarriorState.Chase;
            nextStep = vectorFromAngle(directionToTarget);
            MoveCharacter(chaseSpeed);
        }
        else
        {
            if(isSentry)
            {
                currentState = WarriorState.backToPos;
                float directionToInitalPos = direction(transform.position, initialPosition);
                nextStep = vectorFromAngle(directionToInitalPos);
                MoveCharacter(speed);
            }
            else
            {
                currentState = WarriorState.WalkRandom;
                randomWalk();
            }
        }


    }


    protected override IEnumerator AttackCo()
    {
        launchedAttack = true;
        animator.SetBool("moving", false);
        while (currentState == WarriorState.SpearAttack)
        {
            yield return base.AttackCo();
        }
        launchedAttack = false;
    }


}
