using Assets.Scripts.Hit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Walk,
    Attack,
    Dead
}


namespace Assets.Scripts.Characters
{
    abstract public class Character : MonoBehaviour, IHittable
    {
        protected Mediator mediator = Mediator.Instance;

        [SerializeField] protected float speed;
        [SerializeField] protected float health;
        [SerializeField] private float attackDuration;
        [SerializeField] private float hitCoolDown;
        protected Rigidbody2D myRigidbody;
        protected Animator animator;
        protected Vector3 nextStep;

        protected CharacterState characterState;

        protected bool isHit = false; // prevent multiple hits triggered by a single hit -> multiple collider objects

        virtual public void apply(float damage)
        {
            if (!isHit)
            {
                isHit = true;
                health -= damage;
                if (health <= 0)
                {
                    health = 0;
                    StartCoroutine(DieCo());
                }
                else
                {
                    StartCoroutine(HitCooldownCo());
                }
            }
        }

        private IEnumerator HitCooldownCo()
        {
            // prevent multiple hits triggered by a single hit -> multiple collider objects
            yield return new WaitForSeconds(0.1f);
            isHit = false;
        }

        IEnumerator DieCo()
        {
            characterState = CharacterState.Dead;
            animator.SetBool("attacking", false);
            animator.SetBool("moving", false);
            animator.SetBool("die", true);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }

        virtual protected IEnumerator AttackCo()
        {
            if (characterState != CharacterState.Dead)
            {
                
                animator.SetBool("attacking", true);
                characterState = CharacterState.Attack;
                yield return new WaitForSeconds(attackDuration); // attack duration wait
                animator.SetBool("attacking", false);
                yield return new WaitForSeconds(hitCoolDown);    // attack cool down wait

                // the character could have be killed during his attack cooldown (very likely)
                // we must not change his state to walk if he is dead, thus the codition must be rechecked
                if (characterState != CharacterState.Dead) characterState = CharacterState.Walk;
            }
        }

        virtual protected Vector3 MoveCharacter(float speed)
        {
            Vector3 newPosition = transform.position;
            if(characterState != CharacterState.Dead) { 
                nextStep.Normalize();
                animator.SetFloat("moveX", nextStep.x);
                animator.SetFloat("moveY", nextStep.y);
                animator.SetBool("moving", true);
                newPosition += nextStep * speed * Time.deltaTime;
                if(newPosition != transform.position) animator.SetBool("moving", true);
                myRigidbody.MovePosition(newPosition);
            }
            return newPosition; 
        }

    }
}
