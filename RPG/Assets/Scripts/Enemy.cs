using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    WalkRandom,
    WalkInDirection,
    SpearAttack,
    Interact,
    Die
}
public class Enemy : MonoBehaviour, IHittable
{

    [SerializeField] private float walkSpeed;
    private float fightSpeed;
    private float health;
    private bool isHit = false; // prevent multiple hits triggered by a single hit -> multiple collider objects

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private EnemyState currentState;
    private Mediator mediator = Mediator.Instance;

    private const float visibility      = 5f;
    private const float fightDistance   = 1.5f;
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
        if (currentState == EnemyState.Die) return;

        distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
        directionToPlayer = direction(transform.position, playerPosition);
        if (distanceToPlayer < fightDistance)
        {
            currentState = EnemyState.SpearAttack;
        }
        else if (distanceToPlayer < visibility)
        {
            currentState = EnemyState.WalkInDirection;
        }
        else
        {
            currentState = EnemyState.WalkRandom;
        }


        switch (currentState) {
            case EnemyState.WalkRandom:
                randomWalk();
                break;

            case EnemyState.WalkInDirection:
                change = vectorFromAngle(directionToPlayer);
                MoveCharacter();
                break;

            case EnemyState.SpearAttack:
                currentState = EnemyState.SpearAttack;
                if(!launchedAttack) StartCoroutine(AttackCo());
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
        launchedAttack = true;
        animator.SetBool("moving", false);
        while(currentState == EnemyState.SpearAttack) { 
            animator.SetBool("attacking", true);
            yield return null; // cool down
            animator.SetBool("attacking", false);
            yield return new WaitForSeconds(1.0f); // cool down
        }
        launchedAttack = false;
    }

    private void OnPlayerChangePosition(PlayerChangePositionCommand c)
    {
        playerPosition = c.Position;       
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

    public void apply(float damage)
    {
        if (!isHit)
        {
            isHit = true;
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                
                currentState = EnemyState.Die;
                StartCoroutine(DieCo());
            }
            else
            {
                StartCoroutine(HitCooldownCo());
            }
        }
    }

    private IEnumerator HitCooldownCo()
    {
        // prevent multiple hits triggered by a single hit -> multiple collider objects
        yield return new WaitForSeconds(0.1f);
        isHit = false;
    }

    IEnumerator DieCo()
    {
        animator.SetBool("attacking", false);
        animator.SetBool("moving", false);
        animator.SetBool("die", true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
