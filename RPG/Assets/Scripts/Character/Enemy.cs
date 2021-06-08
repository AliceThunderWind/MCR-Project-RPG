using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    WalkRandom,
    Chase,
    SpearAttack
}
public class Enemy : Character, IFighter
{

    private float chaseSpeed;
    private float chaseSpeedMultiplicator = 4f;
            
    private EnemyState currentState;

    private const float visibility      = 5f;
    private const float fightDistance   = 1.45f;
    private bool launchedAttack         = false;
    private float distanceToPlayer;
    private float directionToPlayer;
    private Vector3 playerPosition;
    
    // for random walk in 5 second periode with 5s pause
    private const float period      = 5f;
    private float nextStartTime     = 5.0f;
    private float nextStopTime;
    

    private static readonly System.Random getrandom = new System.Random(DateTime.Now.Millisecond);

    // Start is called before the first frame update
    void Start()
    {

        animator        = GetComponent<Animator>();
        myRigidbody     = GetComponent<Rigidbody2D>();
        currentState    = EnemyState.WalkRandom;
        
        health          = 100f;
        chaseSpeed      = chaseSpeedMultiplicator * speed;
        nextStopTime    = nextStartTime + period;

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
            currentState = EnemyState.SpearAttack;
            if (!launchedAttack) StartCoroutine(AttackCo());
        }
        else if (distanceToPlayer < visibility)
        {
            currentState = EnemyState.Chase;
            nextStep = vectorFromAngle(directionToPlayer);
            MoveCharacter(chaseSpeed);
        }
        else
        {
            currentState = EnemyState.WalkRandom;
            randomWalk();
        }

    }

    private void randomWalk()
    {
        // Debug.Log("Time : " + Time.time + " start : " + nextStartTime + " stop : " + nextStopTime);
        animator.SetBool("moving", false);
        if (Time.time > nextStartTime && Time.time < nextStopTime)
        {
            if (nextStep != Vector3.zero)
            {
                MoveCharacter(speed);  
            }
        }


        if (Time.time > nextStopTime)
        {

            nextStep = RandomVector();
            nextStartTime = Time.time + period;
            nextStopTime = Time.time + 2 * period;
        }
    }

    protected override IEnumerator AttackCo()
    {
        launchedAttack = true;
        animator.SetBool("moving", false);
        while(currentState == EnemyState.SpearAttack) {
            yield return base.AttackCo();
        }
        launchedAttack = false;
    }

    private void OnPlayerChangePosition(PlayerChangePositionCommand c)
    {
        playerPosition = c.Position;       
    }

    private Vector3 RandomVector()
    {
        float random = UnityEngine.Random.Range(0f, 360f);
        return vectorFromAngle(random);
    }

    private float direction(Vector3 from, Vector3 to)
    {
        float dir = Vector3.Angle(to - from, Vector3.right);
        if (to.y < from.y) dir *= -1;
        return dir;
    }

    private Vector3 vectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }

}
