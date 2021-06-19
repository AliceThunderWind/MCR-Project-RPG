using Assets.Scripts.Character.Selector;
using Assets.Scripts.Hit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Mediator
{
       
    public sealed class GameMediator : MonoBehaviour
    {
        public enum CharacterClass
        {
            Warrior,
            Archer,
            Wizzard
        }

        public enum Level
        {
            Level1,
            Level2,
            Level3
        }

       
        private CommandDispatcher command = CommandDispatcher.Instance;


        public CharacterClass PlayerClass { get; set; }
        public Level PlayerLevel { get; set; }
        private List<Enemy> enemies = new List<Enemy>();
        private List<Characters.Character> allies = new List<Characters.Character>();
        private List<Enemy> sentries = new List<Enemy>();
        
        [SerializeField] public Player player;
        [SerializeField] private GUIHPHandler GUIhp;
        [SerializeField] private PlayerSelector PlayerSelector;
        [SerializeField] private GameObject PlayersContainer;
        [SerializeField] private GameObject AllyAI;
        [SerializeField] private CameraMovement MainCamera;
        [SerializeField] private Gate level1Gate;

        internal void PlayerChangeLevel(Collider2D other, GameObject exit)
        {
            Player p = other.GetComponent<Player>();
            if (p != null && sentries.Count == 0)
            {
                player.Selected = false;
                PlayerLevel = Level.Level2;
                player = null;
                enemies.Clear();
                PlayerPrefs.SetInt("CharacterLevel", PlayerPrefs.GetInt("CharacterClass") + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public Player getPlayer()
        {
            return player;
        }

        
        public Vector3 PlayerPosition { get { return player.Position; } }
        private void Awake()
        {
            command.Subscribe<HitBreakeableCommand>(OnHitBreakable);
            command.Subscribe<HpIncreaseCommand>(OnHpIncrease);
            command.Subscribe<HpDecreaseCommand>(OnHpDecrease);
            command.Subscribe<KnockbackCommand>(OnKnockback);
          

            if (player == null)
            {
                
                enablePlayerSelector();
            }
            else
            {
                player.Selected = true;
                MainCamera.SetTarget(player.transform);
                GUIhp.gameObject.SetActive(true);
                PlayerSelector.gameObject.SetActive(false);
            }
        }

        internal void SelectPlayer(Player player)
        {
            this.player = player;
            player.Selected = true;
            MainCamera.SetTarget(this.player.transform);
            PlayerSelector.gameObject.SetActive(false);
            GUIhp.gameObject.SetActive(true);
        }

        private void enablePlayerSelector()
        {
            MainCamera.SetTarget(PlayersContainer.transform);
            GUIhp.gameObject.SetActive(false);
            PlayerSelector.gameObject.SetActive(true);
        }
        public void PlayerChangeHp(float newHp)
        {
            GUIhp.setHp(newHp);
        }

        internal void registerAlly(Characters.Character ally)
        {
            allies.Add(ally);
        }

        public void unregisterAlly(Characters.Character ally)
        {
            allies.Remove(ally);
        }

        public void registerEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
            if (enemy.IsSentry)
                sentries.Add(enemy);
        }

        public void unregisterEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            if (enemy.IsSentry) { 
                sentries.Remove(enemy);
                if(sentries.Count == 0)
                {
                    level1Gate.isOpen = true;
                }
            }
        }

        private void OnHitBreakable(HitBreakeableCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().Brake();
        }

        public void CharachterHit(ICharacter character, float damage)
        {
            if (character != null)
                character.Damage(damage);
        }

        public void CharacterConfuse(ICharacter character)
        {
            if (character.GetType().IsSubclassOf(typeof(Enemy))) {
                Enemy enemy = (Enemy) character;
                unregisterEnemy(enemy);
                Destroy(enemy.gameObject.GetComponent<Enemy>());
                Ally newAlly = enemy.gameObject.AddComponent<Ally>();
                newAlly.Mediator = this;
                registerAlly(newAlly);
            }
        }

        private void OnHpIncrease(HpIncreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().heal(cmd.Hp);
        }

        private void OnHpDecrease(HpDecreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().Damage(cmd.Hp);
        }

        private void OnKnockback(KnockbackCommand cmd)
        {
            cmd.body.AddForce(cmd.force, ForceMode2D.Impulse);
        }

        public void EnemyBehaviour(Enemy enemy)
        {
            if (player == null || enemy.IsHit || enemy.CharacterState == CharacterState.Dead)
            {
                enemy.setState(EnemyState.NoAction, Vector3.zero);
                return;
            }

            Characters.Character toAttack = player;
            EnemyState nextState;
            Vector3 vectorToTarget;
            float distanceToTarget;
            
            distanceToTarget = Vector3.Distance(enemy.Position, toAttack.Position);
            if(allies.Count > 0) { 
                /* Search for Players Allies to attack them */
                Characters.Character closestAlly = FindClosest(enemy, allies);
                float distanceToClosestAlly = Vector3.Distance(enemy.Position, closestAlly.Position);
                if (distanceToTarget > distanceToClosestAlly)
                {
                    toAttack = closestAlly;
                    distanceToTarget = distanceToClosestAlly;
                }
            }
                   
            nextState = DecideEnemyState(enemy, distanceToTarget);
            
            if(nextState == EnemyState.BackToPos)
            {
                if(Vector3.Distance(enemy.Position, enemy.InitialPosition) < 1)
                {
                    nextState = EnemyState.NoAction;
                    vectorToTarget = Vector3.zero;
                }
                else
                {
                    vectorToTarget = VectorFromAngle(Direction(enemy.Position, enemy.InitialPosition));
                }
            }
            else
            {
                vectorToTarget = VectorFromAngle(Direction(enemy.Position, toAttack.Position));
            }

            enemy.setState(nextState, vectorToTarget);
            
        }

        private Characters.Character FindClosest(Characters.Character from, List<Characters.Character> within)
        {
            float minDistance = float.MaxValue;
            Characters.Character closest = null;
            foreach (Characters.Character character in within)
            {
                float currentDistance = Vector3.Distance(from.Position, character.Position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closest = character;
                }
            }
            return closest;
        }

        private EnemyState DecideEnemyState(Enemy enemy, float distanceToTarget)
        {
            if (distanceToTarget < enemy.FightDistance)
            {
                return EnemyState.Attack;
            }
            else if (distanceToTarget < enemy.Visibility)
            {
                return EnemyState.Chase;
            }
            else
            {
                if (enemy.IsSentry)
                {
                    return EnemyState.BackToPos;
                }
            }
            return EnemyState.WalkRandom;
        }

        public Enemy FindClosestEnemy(Assets.Scripts.Characters.Character character)
        {
            Enemy closestEnemy = null;

            foreach(Enemy e in enemies)
            {
                if(closestEnemy == null || (Vector2.Distance(character.Position, e.Position) < Vector2.Distance(character.Position, closestEnemy.Position) && e != character))
                {
                    closestEnemy = e;
                }
            }
            return closestEnemy;
        }


        internal void AllyBehaviour(Ally wizzardSummon)
        {
            Enemy enemy = FindClosestEnemy(wizzardSummon);
            if(enemy != null)
            {
                AllyState nextState;
                Vector3 vectorToTarget;
                float distanceToTarget;

                distanceToTarget = Vector3.Distance(wizzardSummon.Position, enemy.Position);
                nextState = DecideAllyState(wizzardSummon, distanceToTarget);

                if (nextState == AllyState.Gard)
                {
                    if (Vector3.Distance(wizzardSummon.Position, player.Position) < 3.5f)
                    {
                        nextState = AllyState.NoAction;
                        vectorToTarget = Vector3.zero;
                    }
                    else
                    {
                        vectorToTarget = VectorFromAngle(Direction(wizzardSummon.Position, player.Position));
                    }
                }
                else
                {
                    vectorToTarget = VectorFromAngle(Direction(wizzardSummon.Position, enemy.Position));
                }

                wizzardSummon.setState(nextState, vectorToTarget);
            }
        }

        private AllyState DecideAllyState(Ally ally, float distanceToTarget)
        {
            if (distanceToTarget < ally.FightDistance)
            {
                return AllyState.Attack;
            }
            else if (distanceToTarget < ally.Visibility)
            {
                return AllyState.Chase;
            }
           
            return AllyState.Gard;
        }

        private static float Direction(Vector3 from, Vector3 to)
        {
            float dir = Vector3.Angle(to - from, Vector3.right);
            if (to.y < from.y) dir *= -1;
            return dir;
        }

        public static Vector3 RandomVector()
        {
            float random = UnityEngine.Random.Range(0f, 360f);
            return VectorFromAngle(random);
        }


        public static Vector3 VectorFromAngle(float angle)
        {
            return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        }
    }
}
