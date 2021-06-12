using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using Assets.Scripts.Mediator;
using UnityEngine;
public enum EnemyState
{
    WalkRandom,
    Chase,
    Attack,
    BackToPos,
    NoAction
}
abstract public class Enemy : Character, ICharacter
{
    [SerializeField] protected float Visibility;
    [SerializeField] protected float FightDistance;
    [SerializeField] private float chaseSpeed;

    protected float nextStartTime = 5.0f;
    protected float nextStopTime;
    [SerializeField] protected float period = 5f;
    [SerializeField] public bool IsSentry { get; }
    [SerializeField] public bool GetConfuse { get; }

    
    public Vector3 InitialPosition { get; internal set; }
    protected EnemyState currentState;

    protected bool launchedAttack = false;

    public float GetVisibility
    {
        get { return Visibility; }
    }
    public float GetFightDistance
    {
        get { return FightDistance; }
    }

    public Vector3 GetPlayerPosition
    {
        get { return mediator.PlayerPosition; }
    }

    protected virtual void Start()
    {
        InitialPosition = transform.position;
        RegisterEnemyCommand cmd = new RegisterEnemyCommand();
        cmd.who = this;
        command.Publish(cmd);
    }

    void Update()
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
        }

    }

    public void setState(EnemyState state, Vector3 vectorToTarget)
    {
        this.currentState = state;
        if (state != EnemyState.WalkRandom) this.vectorToTarget = vectorToTarget;
    }

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

    // for random walk in 5 second periode with 5s pause


    // Start is called before the first frame update


    // Update is called once per frame



}
