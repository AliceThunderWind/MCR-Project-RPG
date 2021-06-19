﻿using Assets.Scripts.Mediator;
using Assets.Scripts.Characters;
using UnityEngine;
using Assets.Scripts.Hit;
using Assets.Scripts.Wizzard;
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
    public WizzardCreatureSlot mySlot { get; set; }
    public GameMediator Mediator { get; set; }

    [SerializeField] private float Visibility;
    [SerializeField] private float FightDistance;
    [SerializeField] private float chaseSpeed;
    private bool launchedAttack;
    private bool registered = false;

    public float GetVisibility
    {
        get { return Visibility; }
    }
    public float GetFightDistance
    {
        get { return FightDistance; }
    }

  
    public override void Update()
    {
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
        mySlot.Empty = true;
        return base.DieCo();
    }


}
