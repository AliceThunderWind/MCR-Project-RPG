using Assets.Scripts.Characters;
using UnityEngine;
using Assets.Scripts.Hit;

using System.Collections;

public enum AllyState
{
    Gard,
    Chase,
    Attack,
    NoAction
}
public class Ally : Character, ICharacter
{
        
    private AllyState allyState;
    
    [SerializeField] private float visibility = 10f;
    [SerializeField] private float fightDistance = 1.4f;
    [SerializeField] private float chaseSpeed = 8f;
    [SerializeField] private float lifeDuration = 10f;
    private bool launchedAttack;
    private float LifeStart;

    public float Visibility
    {
        get { return visibility; }
    }
    public float FightDistance
    {
        get { return fightDistance; }
    }

    public void Awake()
    {
        LifeStart = Time.time;
    }

    public override void Update()
    {
        if(Mediator == null) return;
        if(LifeStart + lifeDuration < Time.time)
        {
            Damage(100);
            return;
        }

        Mediator.AllyBehaviour(this);
        switch (allyState)
        {
            case AllyState.Chase:
                MoveCharacter(chaseSpeed);
                break;
            case AllyState.Gard:
                MoveCharacter(speed);
                break;
            case AllyState.Attack:
                if (!launchedAttack) StartCoroutine(AttackCo());
                break;
            case AllyState.NoAction:
                animator.SetBool("moving", false);
                break;

        }
    }

    public void setState(AllyState state, Vector3 vectorToTarget)
    {
        this.allyState = state;
        this.vectorToTarget = vectorToTarget;
    }

    protected override IEnumerator AttackCo()
    {
        launchedAttack = true;
        yield return base.AttackCo();
        launchedAttack = false;
    }
    protected override IEnumerator DieCo()
    {
        Mediator.unregisterAlly(this);
        yield return base.DieCo();
        Destroy(gameObject, 2f);
    }


}

