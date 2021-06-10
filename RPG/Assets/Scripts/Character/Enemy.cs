using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

abstract public class Enemy : Character, ICharacter
{
    [SerializeField] protected float visibility;
    [SerializeField] protected float fightDistance;
    protected float nextStartTime = 5.0f;
    protected float nextStopTime;
    [SerializeField] protected float period = 5f;
    [SerializeField] protected bool isSentry;
    protected Vector3 playerPosition;
    protected Vector3 initialPosition;


    protected virtual void Start()
    {
        mediator.Subscribe<PlayerChangePositionCommand>(OnPlayerChangePosition);
        initialPosition = transform.position;
    }

    void Update()
    {
        
    }

    protected void randomWalk()
    {
        // Debug.Log("Time : " + Time.time + " start : " + nextStartTime + " stop : " + nextStopTime);
        animator.SetBool("moving", false);
        if (Time.time > nextStartTime && Time.time < nextStopTime)
        {
            if (nextStep != Vector3.zero)
            {
                MoveCharacter(speed);
            }
        }


        if (Time.time > nextStopTime)
        {

            nextStep = RandomVector();
            nextStartTime = Time.time + period;
            nextStopTime = Time.time + 2 * period;
        }
    }

    protected void OnPlayerChangePosition(PlayerChangePositionCommand c)
    {
        playerPosition = c.Position;
    }

    protected Vector3 RandomVector()
    {
        float random = UnityEngine.Random.Range(0f, 360f);
        return vectorFromAngle(random);
    }

    protected float direction(Vector3 from, Vector3 to)
    {
        float dir = Vector3.Angle(to - from, Vector3.right);
        if (to.y < from.y) dir *= -1;
        return dir;
    }

    protected Vector3 vectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }

    public Vector3 getPlayerPosition()
    {
        return playerPosition;
    }
    // for random walk in 5 second periode with 5s pause


    // Start is called before the first frame update


    // Update is called once per frame

}
