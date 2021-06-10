using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

public enum WarriorState
{
    WalkRandom,
    Chase,
    SpearAttack
}
public class Warrior : Enemy
{
    [SerializeField] private float chaseSpeed;

    private WarriorState currentState;



    private bool launchedAttack = false;
    private float distanceToPlayer;
    private float directionToPlayer;


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

        distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
        directionToPlayer = direction(transform.position, playerPosition);

        if (distanceToPlayer < fightDistance)
        {
            currentState = WarriorState.SpearAttack;
            if (!launchedAttack) StartCoroutine(AttackCo());
        }
        else if (distanceToPlayer < visibility)
        {
            currentState = WarriorState.Chase;
            nextStep = vectorFromAngle(directionToPlayer);
            MoveCharacter(chaseSpeed);
        }
        else
        {
            currentState = WarriorState.WalkRandom;
            randomWalk();
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
