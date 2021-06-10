using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

public class Player : Character, ICharacter
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

        PlayerChangePositionCommand cmd = new PlayerChangePositionCommand();
        cmd.Position = transform.position;
        mediator.Publish(cmd);

        StartCoroutine(DisplayHp());
    }

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = this.health;
        mediator.Publish(cmd);
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

    public override void damage(float damage)
    {
        base.damage(damage);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = health;
        mediator.Publish(cmd);
    }

    public override void heal(float damage)
    {
        base.heal(damage);
        StartCoroutine(DisplayHp());
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
