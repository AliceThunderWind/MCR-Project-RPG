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
    
    [SerializeField] private float chaseSpeed;
    

    private WarriorState currentState;

    private float directionToTarget;
    private bool launchedAttack = false;
    
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
        //command.Subscribe<PlayerChangePositionCommand>(OnPlayerChangePosition);
        
    }

    public void setState(WarriorState state, float directionToTarget)
    {
        this.currentState = state;
        this.directionToTarget = directionToTarget;
    }

    // Update is called once per frame
    void Update()
    {
        mediator.WarriorBehaviour(this);
        switch (currentState)
        {
            case WarriorState.Chase:
                nextStep = vectorFromAngle(directionToTarget);
                MoveCharacter(chaseSpeed);
                break;
            case WarriorState.BackToPos:
                nextStep = vectorFromAngle(directionToTarget);
                MoveCharacter(speed);
                break;
            case WarriorState.MeleeAttack:
                if (!launchedAttack) StartCoroutine(AttackCo());
                break;
            case WarriorState.WalkRandom:
                randomWalk();
                break;
            case WarriorState.NoAction:
                animator.SetBool("moving", false);
                break;
        }

    }


    protected override IEnumerator AttackCo()
    {
        launchedAttack = true;
        animator.SetBool("moving", false);
        while (currentState == WarriorState.MeleeAttack)
        {
            yield return base.AttackCo();
        }
        launchedAttack = false;
    }


}
