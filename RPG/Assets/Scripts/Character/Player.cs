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
        CharacterState = CharacterState.Idle;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        animator.SetBool("moving", false);
        health = 100f;

        PlayerChangePositionCommand cmd = new PlayerChangePositionCommand();
        cmd.Position = transform.position;
        command.Publish(cmd);

        StartCoroutine(DisplayHp());
    }

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = this.health;
        command.Publish(cmd);
    }



    // Update is called once per frame
    void Update()
    {
        animator.SetBool("moving", false);
        vectorToTarget = Vector3.zero;
        vectorToTarget.x = Input.GetAxisRaw("Horizontal");
        vectorToTarget.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("SwordAttack") && CharacterState != CharacterState.Attack)
        {
            StartCoroutine(AttackCo());
        }

        if (vectorToTarget != Vector3.zero)
        {
            MoveCharacter(speed);
        }

    }

    public override void damage(float damage)
    {
        base.damage(damage);
        HpDisplayCommand cmd = new HpDisplayCommand();
        cmd.Hp = health;
        command.Publish(cmd);
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
            command.Publish(cmd);
        }
        return newPosition;
    }

}
