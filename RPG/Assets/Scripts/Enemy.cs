using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Walk,
    SpearAttack,
    Interact
}
public class Enemy : MonoBehaviour
{

    public float speed;
    public float health;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private EnemyState currentState;
    private Mediator mediator = Mediator.Instance;

    private float nextActionTime = 5.0f;
    private float nextStopTime;
    private float period = 5f;

    private static readonly System.Random getrandom = new System.Random(DateTime.Now.Millisecond);

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);
        health = 100f;
        nextStopTime = nextActionTime + period;

        change = RandomVector();

    }

    private Vector3 RandomVector()
    {
        float random = UnityEngine.Random.Range(0f, 260f);
        Vector3 vec = new Vector3(Mathf.Cos(random), Mathf.Sin(random), 0);
        vec.Normalize();
        return vec;
    }
   

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time + " | act time : " + nextActionTime + " stop time : " + nextStopTime);
        
        if (Time.time > nextActionTime && Time.time < nextStopTime)
        {
            if (change != Vector3.zero) { 
                MoveCharacter();
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
                animator.SetBool("moving", true);
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
     

        if(Time.time > nextStopTime + period)
        {

            change = RandomVector();

            nextActionTime = Time.time + period;
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
        currentState = EnemyState.Walk;
    }

   
    void MoveCharacter()
    {
        change.Normalize();
        Vector3 newPosition = transform.position + change * speed * Time.deltaTime;
        myRigidbody.MovePosition(newPosition);
        
       
    }
}
