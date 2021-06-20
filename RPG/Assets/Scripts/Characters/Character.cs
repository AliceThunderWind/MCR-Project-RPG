using Assets.Scripts.Hit;
using Assets.Scripts.Mediator;
using System.Collections;
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
    abstract public class Character : MonoBehaviour, ICharacter
    {
        protected CommandDispatcher command = CommandDispatcher.Instance;
        [SerializeField] protected GameMediator mediator;
        public GameMediator Mediator { get { return this.mediator; } set { this.mediator = value; } }
        public Vector3 Position {
            get {
                return transform.position; 
            } 
            set { 
                transform.position = value; 
            }
        }

        public bool IsHit { get; set; } = false; // prevent multiple hits triggered by a single hit -> multiple collider objects

        [SerializeField] protected float speed = 7f;
        [SerializeField] protected float health = 100f;
        [SerializeField] protected float attackDuration = 0.8f;
        [SerializeField] protected float attackCoolDown = 0f;
        protected Rigidbody2D myRigidbody;
        protected Animator animator;
        protected Vector3 vectorToTarget;

        public Character ClosestEnemy { get; set; }

        public CharacterState CharacterState { get; internal set; }

        public float HP { get { return health; } }

        virtual protected void Start()
        {
            animator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody2D>();
            CharacterState = CharacterState.Idle;
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", -1);
            animator.SetBool("moving", false);
            health = 100f;

           
        }

        virtual public float Damage(float damage)
        {
            if (!IsHit)
            {
                IsHit = true;
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
            return health;
        }


        virtual public float heal(float hp)
        {
            this.health += hp;
            if (this.health > 100) this.health = 100;
            return health;

        }

        private IEnumerator HitCooldownCo()
        {
            // prevent multiple hits triggered by a single hit -> multiple collider objects
            yield return new WaitForSeconds(0.1f);
            IsHit = false;
        }

        virtual protected IEnumerator DieCo()
        {
            CharacterState = CharacterState.Dead;
            animator.SetBool("attacking", false);
            animator.SetBool("moving", false);
            animator.SetTrigger("die");
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            
        }

        virtual protected IEnumerator AttackCo()
        {
            if (CharacterState != CharacterState.Dead)
            {

                CharacterState = CharacterState.Attack;
                animator.SetBool("attacking", true);
                yield return new WaitForSeconds(attackDuration); // attack duration wait
                animator.SetBool("attacking", false);
                yield return new WaitForSeconds(attackCoolDown);    // attack cool down wait
                // the character could have be killed during his attack cooldown (very likely)
                // we must not change his state to walk if he is dead, thus the codition must be rechecked
                if (CharacterState != CharacterState.Dead) CharacterState = CharacterState.Walk;
            }
        }

        virtual protected Vector3 MoveCharacter(float speed)
        {
            Vector3 newPosition = transform.position;
            if(CharacterState != CharacterState.Dead) {
                vectorToTarget.Normalize();
                animator.SetFloat("moveX", vectorToTarget.x);
                animator.SetFloat("moveY", vectorToTarget.y);
                animator.SetBool("moving", true);
                newPosition += vectorToTarget * speed * Time.deltaTime;
                if(newPosition != transform.position) animator.SetBool("moving", true);
                myRigidbody.MovePosition(newPosition);
            }
            return newPosition; 
        }

        public abstract void Update();

        public GameMediator getMediator()
        {
            return mediator;
        }

     
    }
}
