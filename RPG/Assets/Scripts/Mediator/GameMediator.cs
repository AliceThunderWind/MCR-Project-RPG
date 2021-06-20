using Assets.Scripts.Character.Selector;
using Assets.Scripts.Hit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Mediator
{
       
    /// <summary>
    /// Classe représentant notre Mediator
    /// </summary>
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
        [SerializeField] private WeaponEquipped weaponEquipped;
        [SerializeField] private PlayerSelector PlayerSelector;
        [SerializeField] private GameObject PlayersContainer;
        [SerializeField] private CameraMovement MainCamera;
        [SerializeField] private Gate level1Gate;

        private string selectedCharacterDataName = "CharacterClass";
        private string selectedCharacterLevel = "CharacterLevel";

        internal void PlayerChangeLevel(Collider2D other, GameObject exit)
        {
            Player p = other.GetComponent<Player>();
            if (p != null && sentries.Count == 0)
            {
                //PlayerLevel++;
                player = null;
                enemies.Clear();
                PlayerPrefs.SetInt(selectedCharacterLevel, (int)PlayerLevel);
                PlayerPrefs.SetInt(selectedCharacterDataName, (int)PlayerClass);
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

            PlayerLevel = (Level)PlayerPrefs.GetInt(selectedCharacterLevel, 0);
            PlayerClass = (CharacterClass)PlayerPrefs.GetInt(selectedCharacterDataName, 0);

            if (player == null)
            {   
                enablePlayerSelector();
                weaponEquipped.changeText(PlayerSelector.PlayerWeaponName());
            }
            else
            {
                MainCamera.SetTarget(player.transform);
                GUIhp.gameObject.SetActive(true);
                PlayerSelector.gameObject.SetActive(false);
            }
        }

        public void changeWeapon(int direction, float health)
        {
            Vector3 oldPosition = player.Position;
            string weapon = PlayerSelector.ChangeWeapon(direction);
            weaponEquipped.changeText(weapon);
            player.transform.position = oldPosition;
            player.setHealth(health);
        }
        /*
        internal void SelectPlayer(Player player)
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
        */
        public void SelectPlayer(Player player)
        {
            if (this.player != null) this.player.Selected = false;
            this.player = player;
            this.player.Selected = true;
            MainCamera.SetTarget(this.player.transform);
            //PlayerSelector.gameObject.SetActive(false);
            //GUIhp.gameObject.SetActive(true);
        }

        private void enablePlayerSelector()
        {
            MainCamera.SetTarget(PlayersContainer.transform);
            GUIhp.gameObject.SetActive(true);
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


        public void CharachterHit(ICharacter character, float damage)
        {
            if (character != null)
                character.Damage(damage);
        }

        /// <summary>
        /// Méthode permettant de changer le comportement d'un ennemi à un allié
        /// </summary>
        /// <param name="character"></param>
        public void CharacterConfuse(ICharacter character)
        {
            if (character.GetType().IsSubclassOf(typeof(Enemy))) {
                Enemy enemy = (Enemy) character;
                if (!enemy.IsSentry) { 
                    unregisterEnemy(enemy);
                    Destroy(enemy.gameObject.GetComponent<Enemy>());
                    Ally newAlly = enemy.gameObject.AddComponent<Ally>();
                    newAlly.Mediator = this;
                    registerAlly(newAlly);
                }
            }
        }
        private void OnHitBreakable(HitBreakeableCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().Brake();
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
        /// <summary>
        /// Méthode permettant d'exécuter le comportement d'un ennemi
        /// </summary>
        /// <param name="enemy"></param>
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

        /// <summary>
        /// Permet de décider l'état d'un ennemi
        /// </summary>
        /// <param name="enemy">Ennemi</param>
        /// <param name="distanceToTarget">Distance à la cible</param>
        /// <returns>Nouvel état</returns>
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

       

        /// <summary>
        /// Permet de décider du comportement d'un Ally
        /// </summary>
        /// <param name="wizzardSummon">Allié</param>
        internal void AllyBehaviour(Ally wizzardSummon)
        {
            Enemy enemy = FindClosestEnemy(wizzardSummon);
            float distanceToTarget;
            AllyState nextState = AllyState.Gard;
            Vector3 vectorToTarget;

            if (enemy != null)
            {
                
                distanceToTarget = Vector3.Distance(wizzardSummon.Position, enemy.Position);
                nextState = DecideAllyState(wizzardSummon, distanceToTarget);
            }

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
        
        /// <summary>
        /// Permet de changer l'état des alliés
        /// </summary>
        /// <param name="ally">Allié concerné</param>
        /// <param name="distanceToTarget">Distance par rapport à la cible</param>
        /// <returns>Le nouvel état</returns>
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

        /// <summary>
        /// Permet de récupérer le Character le plus proche dans une liste
        /// </summary>
        /// <param name="from">Source</param>
        /// <param name="within">Liste dans laquelle chercher</param>
        /// <returns>Le plus proche</returns>
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

        /// <summary>
        /// Méthode permettant de récupérer l'ennemi le plus proche
        /// </summary>
        /// <param name="character">Source à partir de laquelle chercher</param>
        /// <returns>L'ennemi le plus proche</returns>
        public Enemy FindClosestEnemy(Assets.Scripts.Characters.Character character)
        {
            Enemy closestEnemy = null;
            foreach (Enemy e in enemies)
            {
                if (closestEnemy == null || (Vector2.Distance(character.Position, e.Position) < Vector2.Distance(character.Position, closestEnemy.Position) && e != character))
                {
                    closestEnemy = e;
                }
            }
            return closestEnemy;
        }

        /// <summary>
        /// Permet de récupérer un Vector3 représentant la différence de position entre from et to
        /// </summary>
        /// <param name="from">Source</param>
        /// <param name="to">Cible</param>
        /// <returns>Le Vector3</returns>
        private static float Direction(Vector3 from, Vector3 to)
        {
            float dir = Vector3.Angle(to - from, Vector3.right);
            if (to.y < from.y) dir *= -1;
            return dir;
        }

        /// <summary>
        /// Permet de récupérer un vecteur aléatoire
        /// </summary>
        /// <returns>Un vecteur aléatoire</returns>
        public static Vector3 RandomVector()
        {
            float random = Random.Range(0f, 360f);
            return VectorFromAngle(random);
        }

        /// <summary>
        /// Méthode permettant de récupérer un Vector3 à partie d'un angle
        /// </summary>
        /// <param name="angle">L'angle</param>
        /// <returns>Le Vector3 correspondant</returns>
        public static Vector3 VectorFromAngle(float angle)
        {
            return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        }
    }
}
