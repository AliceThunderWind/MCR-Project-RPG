using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

public class Player : Character, IHittable
{
    

    private float positionUpdateInterval = 0.3f; // delay update in seconds
   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        characterState = CharacterState.Idle;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);
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
        animator.SetBool("moving", false);
        nextStep = Vector3.zero;
        nextStep.x = Input.GetAxisRaw("Horizontal");
        nextStep.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("SwordAttack") && characterState != CharacterState.Attack)
        {
            StartCoroutine(AttackCo());
        }
        
        if (nextStep != Vector3.zero) { 
            MoveCharacter(speed);
        }    

    }

    public override void apply(float damage)
    {
        base.apply(damage);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = health;
        mediator.Publish(cmd);
    }

 

    // static int count = 0;

    protected override Vector3 MoveCharacter(float speed)
    {
        Vector3 newPosition = base.MoveCharacter(speed);
        
        // Pubish new Position (every 300ms) while moving
        if (Time.time >= positionUpdateInterval)
        {
            // Debug.Log("Move Publish : " + ++count + " TIME : " + Time.time);
            positionUpdateInterval = Time.time + 0.3f;

            PlayerChangePositionCommand cmd = new PlayerChangePositionCommand();
            cmd.Position = newPosition;
            mediator.Publish(cmd);
        }
        return newPosition;
    }
    
}
