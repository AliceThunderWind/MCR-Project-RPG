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

    [SerializeField] private const float period = 5f;
    [SerializeField] private float attackCooldown;
    private float nextStartTime = 5.0f;
    private float nextStopTime;
    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = ArcherState.WalkRandom;


        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);

        nextStep = RandomVector();
        mediator.Subscribe<PlayerChangePositionCommand>(OnPlayerChangePosition);
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
}
