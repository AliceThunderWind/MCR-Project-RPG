using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

public class Player : Character, ICharacter
{

    public bool Selected { get; set; } = false;
    // Start is called before the first frame update
    

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        mediator.PlayerChangeHp(health);
    }

    public override void  Start()
    {
        base.Start();
        StartCoroutine(DisplayHp());
    }

    // Update is called once per frame
    void Update()
    {
        if (!Selected) return;

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
