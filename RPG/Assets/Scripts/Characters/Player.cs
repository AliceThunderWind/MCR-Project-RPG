using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

public class Player : Character, ICharacter
{

    // Start is called before the first frame update

    protected KeyCode nextKey = KeyCode.E;
    protected KeyCode previousKey = KeyCode.Q;
    

    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        mediator.PlayerChangeHp(health);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisplayHp());
    }

    // Update is called once per frame
    public override void Update()
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

        if (Input.GetKeyDown(nextKey))
        {
            if(CharacterState != CharacterState.Attack)
                mediator.changeWeapon(1, health);
        }

        if (Input.GetKeyDown(previousKey))
        {
            if (CharacterState != CharacterState.Attack)
                mediator.changeWeapon(-1, health);
        }        

    }

    public void setHealth(float newHealth)
    {
        this.health = newHealth;
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
