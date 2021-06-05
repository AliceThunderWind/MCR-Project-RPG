using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    WalkRandom,
    WalkInDirection,
    SpearAttack,
    Interact
}
public class Enemy : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    private float fightSpeed;
    private float health;

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private EnemyState currentState;
    private Mediator mediator = Mediator.Instance;

    private const float visibility  = 5f;
    private float distanceToPlayer;
    private float directionToPlayer;
    
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
        fightSpeed      = 4 * walkSpeed;
        nextStopTime    = nextStartTime + period;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);

        change = RandomVector();
        mediator.Subscribe<PlayerChangePositionCommand>(OnPlayerChangePosition);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time + " | act time : " + nextActionTime + " stop time : " + nextStopTime);
        switch (currentState) {
            case EnemyState.WalkRandom:
                randomWalk();
                break;
            case EnemyState.WalkInDirection:
                change = vectorFromAngle(directionToPlayer);
                MoveCharacter();
                break;
        }
       
    }

    private void randomWalk()
    {
        // Debug.Log("Time : " + Time.time + " start : " + nextStartTime + " stop : " + nextStopTime);
        if (Time.time > nextStartTime && Time.time < nextStopTime)
        {
            if (change != Vector3.zero)
            {
                MoveCharacter();  
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }
        else
        {
            animator.SetBool("moving", false);
        }


        if (Time.time > nextStopTime)
        {

            change = RandomVector();
            nextStartTime = Time.time + period;
            nextStopTime = Time.time + 2 * period;
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = EnemyState.SpearAttack;
        yield return null; // wait one frame
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(1.0f); // cool down
        currentState = EnemyState.WalkRandom;
    }

    private void OnPlayerChangePosition(PlayerChangePositionCommand c)
    {

        distanceToPlayer = Vector3.Distance(c.Position, transform.position);
        if (distanceToPlayer < visibility)
        {
            currentState = EnemyState.WalkInDirection;
            directionToPlayer = direction(transform.position, c.Position);

        }
        else
        {
            currentState = EnemyState.WalkRandom;
        }
    }


    void MoveCharacter()
    {
        float speed = currentState == EnemyState.WalkRandom ? walkSpeed : fightSpeed;
        animator.SetFloat("moveX", change.x);
        animator.SetFloat("moveY", change.y);
        animator.SetBool("moving", true);
        change.Normalize();
        Vector3 newPosition = transform.position + change * speed * Time.deltaTime;
        myRigidbody.MovePosition(newPosition);
        
       
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
