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
    /// <summary>
    /// Classe représentant un personnage
    /// </summary>
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

        /// <summary>
        /// Méthode appelée juste avant la première frame
        /// </summary>
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

        /// <summary>
        /// Applique un certain nombre de points de dommage
        /// </summary>
        /// <param name="hp">Nombre de points de dommage</param>
        /// <returns>Les points de vie après application</returns>
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

        /// <summary>
        /// Applique un certain nombre de points de soin
        /// </summary>
        /// <param name="hp">Nombre de points à heal</param>
        /// <returns>Les points de vie après application</returns>
        virtual public float heal(float hp)
        {
            this.health += hp;
            if (this.health > 100) this.health = 100;
            return health;

        }

        /// <summary>
        /// Coroutine permettant d'éviter de recevoir plusieurs coups en même temps
        /// </summary>
        /// <returns></returns>
        private IEnumerator HitCooldownCo()
        {
            // prevent multiple hits triggered by a single hit -> multiple collider objects
            yield return new WaitForSeconds(0.1f);
            IsHit = false;
        }

        /// <summary>
        /// Coroutine permettant de faire mourir le personnage
        /// </summary>
        /// <returns></returns>
        virtual protected IEnumerator DieCo()
        {
            CharacterState = CharacterState.Dead;
            animator.SetBool("attacking", false);
            animator.SetBool("moving", false);
            animator.SetTrigger("die");
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            
        }

        /// <summary>
        /// Coroutine permettant d'attaquer
        /// </summary>
        /// <returns></returns>
        virtual protected IEnumerator AttackCo()
        {
            if (CharacterState != CharacterState.Dead)
            {
                
                animator.SetBool("attacking", true);
                CharacterState = CharacterState.Attack;
                yield return new WaitForSeconds(attackDuration); // attack duration wait
                animator.SetBool("attacking", false);
                yield return new WaitForSeconds(attackCoolDown);    // attack cool down wait
                // the character could have be killed during his attack cooldown (very likely)
                // we must not change his state to walk if he is dead, thus the codition must be rechecked
                if (CharacterState != CharacterState.Dead) CharacterState = CharacterState.Walk;
            }
        }

        /// <summary>
        /// Méthode permettant de déplacer le personnage
        /// </summary>
        /// <param name="speed">La vitesse de déplacement</param>
        /// <returns>Nouvelle position</returns>
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

        /// <summary>
        /// Getter du médiateur
        /// </summary>
        /// <returns>Le médiateur</returns>
        public GameMediator getMediator()
        {
            return mediator;
        }

     
    }
}
