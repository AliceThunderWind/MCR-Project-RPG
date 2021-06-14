using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

public class Player : Character, ICharacter
{


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

        StartCoroutine(DisplayHp());
    }

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        mediator.PlayerChangeHp(health);
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

    public override float Damage(float damage)
    {
        float hp = base.Damage(damage);
        StartCoroutine(DisplayHp());
        return hp;
    }

    public override float heal(float damage)
    {
        float hp = base.heal(damage);
        StartCoroutine(DisplayHp());
        return hp;
    }

    // static int count = 0;


}
