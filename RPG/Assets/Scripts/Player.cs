using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Walk,
    SwordAttack,
    Interact
}
public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float health;

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private PlayerState currentState;
    private Mediator mediator = Mediator.Instance;

    private int nextPositionUpdate = 1; // delay update in seconds

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = PlayerState.Walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        health = 100f;
        
        mediator.Subscribe<HpIncreaseCommand>(OnHpIncrease);
        mediator.Subscribe<HpDecreaseCommand>(OnHpDecrease);
        StartCoroutine(DisplayHp());
    }

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = this.health;
        mediator.Publish(cmd);
    }

    private void OnHpIncrease(HpIncreaseCommand c)
    {
        this.health += c.Hp;
        if (this.health > 100) this.health = 100;
        StartCoroutine(DisplayHp());
    }

    private void OnHpDecrease(HpDecreaseCommand c)
    {
        this.health -= c.Hp;
        if (this.health < 0) this.health = 0;
        StartCoroutine(DisplayHp());
    }


    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("SwordAttack") && currentState != PlayerState.SwordAttack)
        {
            StartCoroutine(AttackCo());
        }
        //if(currentState == PlayerState.Walk) { 
            UpdateAnimationAndMove();
        //}
       

    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.SwordAttack;
        yield return null; // wait one frame
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(1.0f); // cool down
        currentState = PlayerState.Walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
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

    static int count = 0;

    void MoveCharacter()
    {
        change.Normalize();
        Vector3 newPosition = transform.position + change * speed * Time.deltaTime;
        myRigidbody.MovePosition(newPosition);
        
        // Pubish new Position (every second)
        if (Time.time >= nextPositionUpdate)
        {
            Debug.Log("Move Publish : " + ++count + " TIME : " + Time.time);
            nextPositionUpdate = Mathf.FloorToInt(Time.time) + 1;

            PlayerChangePositionCommand cmd = new PlayerChangePositionCommand();
            cmd.Position = newPosition;
            mediator.Publish(cmd);
        }
    }
}
