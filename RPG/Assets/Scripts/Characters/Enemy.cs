using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using Assets.Scripts.Mediator;
using System.Collections;
using UnityEngine;
public enum EnemyState
{
    WalkRandom,
    Chase,
    Attack,
    BackToPos,
    NoAction
}

/// <summary>
/// Classe représentant un ennemi
/// </summary>
/// <inheritdoc/>
abstract public class Enemy : Character, ICharacter
{
    [SerializeField] protected float visibility;
    [SerializeField] protected float fightDistance;
    [SerializeField] private float chaseSpeed;

    protected float nextStartTime = 5.0f;
    protected float nextStopTime;
    [SerializeField] protected float period = 5f;
    [SerializeField] private bool sentry;
    public bool IsSentry { get => sentry; }
    
    public Vector3 InitialPosition { get; internal set; }
    protected EnemyState currentState;

    protected bool launchedAttack = false;

    public float Visibility
    {
        get { return visibility; }
    }
    public float FightDistance
    {
        get { return fightDistance; }
    }

    public Vector3 GetPlayerPosition
    {
        get { return mediator.PlayerPosition; }
    }

    protected override void Start()
    {
        base.Start();
        InitialPosition = transform.position;
        mediator.registerEnemy(this);
    }

    public override void Update()
    {
        mediator.EnemyBehaviour(this);
        switch (currentState)
        {
            case EnemyState.Chase:
                MoveCharacter(chaseSpeed);
                break;
            case EnemyState.BackToPos:
                MoveCharacter(speed);
                break;
            case EnemyState.Attack:
                if (!launchedAttack) StartCoroutine(AttackCo());
                break;
            case EnemyState.WalkRandom:
                randomWalk();
                break;
            case EnemyState.NoAction:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
                animator.SetBool("moving", false);
                break;
        }

    }
    /// <summary>
    /// Méthode permettant de changer l'état de l'ennemi
    /// </summary>
    /// <param name="state">Nouvel état</param>
    /// <param name="vectorToTarget">Direction vers la cible</param>
    public void setState(EnemyState state, Vector3 vectorToTarget)
    {
        this.currentState = state;
        if (state != EnemyState.WalkRandom) this.vectorToTarget = vectorToTarget;
    }
    /// <summary>
    /// Méthode permettant de déplacer un ennemi aléatoirement
    /// </summary>
    protected void randomWalk()
    {
        // Debug.Log("Time : " + Time.time + " start : " + nextStartTime + " stop : " + nextStopTime);
        animator.SetBool("moving", false);
        if (Time.time > nextStartTime && Time.time < nextStopTime)
        {
            if (vectorToTarget != Vector3.zero)
            {
                MoveCharacter(speed);
            }
        }

        if (Time.time > nextStopTime)
        {
            vectorToTarget = GameMediator.RandomVector();
            nextStartTime = Time.time + period;
            nextStopTime = Time.time + 2 * period;
        }
    }

    protected override IEnumerator DieCo()
    {
        mediator.unregisterEnemy(this);
        return base.DieCo();
    }

}
