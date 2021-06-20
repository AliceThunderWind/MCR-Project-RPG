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
            Level1 = 0,
            Level2 = 1,
            Level3 = 2
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
        [SerializeField] private Gate levelGate;

        private string selectedCharacterDataName = "CharacterClass";
        private string selectedCharacterLevel = "CharacterLevel";

        /// <summary>
        /// Méthode s'occupant du changement de niveau 
        /// </summary>
        /// <param name="other">Collider en collision</param>
        internal void PlayerChangeLevel(Collider2D other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null && sentries.Count == 0)
            {
                if(PlayerLevel < Level.Level3)
                {
                    PlayerLevel++;
                }
                
                player = null;
                enemies.Clear();
                PlayerPrefs.SetInt(selectedCharacterLevel, (int)PlayerLevel);
                PlayerPrefs.SetInt(selectedCharacterDataName, (int)PlayerClass);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        /// <summary>
        /// Méthode permettant de gérer la mort du joueur et aller au menu mort 
        /// </summary>
        public void playerDeath()
        {
            PlayerPrefs.SetInt(selectedCharacterLevel, (int)PlayerLevel);
            PlayerPrefs.SetInt(selectedCharacterDataName, (int)PlayerClass);
            SceneManager.LoadScene("menuMort");
            
        }

        /// <summary>
        /// Getter du joueur
        /// </summary>
        /// <returns>Le joueur</returns>
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

        /// <summary>
        /// Méthode permettant de changer d'arme
        /// </summary>
        /// <param name="direction">Direction du changement</param>
        /// <param name="health">Points de vie actuels</param>
        public void changeWeapon(int direction, float health)
        {
            Vector3 oldPosition = player.Position;
            string weapon = PlayerSelector.ChangeWeapon(direction);
            weaponEquipped.changeText(weapon);
            player.transform.position = oldPosition;
            player.setHealth(health);
        }


        /// <summary>
        /// Méthode permettant de choisir la classe du joueur
        /// </summary>
        /// <param name="player">Joueur courant</param>
        public void SelectPlayer(Player player)
        {
            if (this.player != null) this.player.Selected = false;
            this.player = player;
            this.player.Selected = true;
            MainCamera.SetTarget(this.player.transform);
        }

        /// <summary>
        /// Méthode permettant d'activer le playerSelector
        /// </summary>
        private void enablePlayerSelector()
        {
            MainCamera.SetTarget(PlayersContainer.transform);
            GUIhp.gameObject.SetActive(true);
            PlayerSelector.gameObject.SetActive(true);
            PlayerSelector.StartOnce();
        }

        /// <summary>
        /// Méthode permettant de dire au GUI de refresh son affichage
        /// </summary>
        /// <param name="newHp">Nouvel valeur des points de vie</param>
        public void PlayerChangeHp(float newHp)
        {
            GUIhp.setHp(newHp);
        }

        /// <summary>
        /// Méthode permettant d'ajouter un allié aux différentes listes
        /// </summary>
        /// <param name="enemy">L'allié à rajouter</param>
        internal void registerAlly(Characters.Character ally)
        {
            allies.Add(ally);
        }

        /// <summary>
        /// Permet de retirer l'allié des différentes listes
        /// </summary>
        /// <param name="enemy">L'allié mort</param>
        public void unregisterAlly(Characters.Character ally)
        {
            allies.Remove(ally);
        }

        /// <summary>
        /// Méthode permettant d'ajouter un ennemi aux différentes listes
        /// </summary>
        /// <param name="enemy">L'ennemi à rajouter</param>
        public void registerEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
            if (enemy.IsSentry)
                sentries.Add(enemy);
        }

        /// <summary>
        /// Permet de retirer l'ennemi des différentes listes
        /// </summary>
        /// <param name="enemy">L'ennemi mort</param>
        public void unregisterEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            if (enemy.IsSentry) { 
                sentries.Remove(enemy);
                if(sentries.Count == 0)
                {
                    levelGate.isOpen = true;
                }
            }
        }

        /// <summary>
        /// Méthode permettant d'appliquer des dommages à un personnage
        /// </summary>
        /// <param name="character">Le personnage conecerné</param>
        /// <param name="damage">La quantité de dommages</param>
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
        /// <summary>
        /// Méthode utilisée pour exécuter certaines actions lors de la publication d'une commande du type du paramètre cmd
        /// </summary>
        /// <param name="cmd">La commande que l'on écoute</param>
        private void OnHitBreakable(HitBreakeableCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().Brake();
        }
        /// <summary>
        /// Méthode utilisée pour exécuter certaines actions lors de la publication d'une commande du type du paramètre cmd
        /// </summary>
        /// <param name="cmd">La commande que l'on écoute</param>
        private void OnHpIncrease(HpIncreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().heal(cmd.Hp);
        }
        /// <summary>
        /// Méthode utilisée pour exécuter certaines actions lors de la publication d'une commande du type du paramètre cmd
        /// </summary>
        /// <param name="cmd">La commande que l'on écoute</param>
        private void OnHpDecrease(HpDecreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().Damage(cmd.Hp);
        }
        /// <summary>
        /// Méthode utilisée pour exécuter certaines actions lors de la publication d'une commande du type du paramètre cmd
        /// </summary>
        /// <param name="cmd">La commande que l'on écoute</param>
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
        /// <param name="ally">Allié</param>
        internal void AllyBehaviour(Ally ally)
        {
            
            Enemy enemy = FindClosestEnemy(ally);
            float distanceToTarget;
            AllyState nextState = AllyState.Gard;
            Vector3 vectorToTarget;

            if (enemy != null)
            {
                
                distanceToTarget = Vector3.Distance(ally.Position, enemy.Position);
                nextState = DecideAllyState(ally, distanceToTarget);
            }

            if (nextState == AllyState.Gard)
            {
                if (Vector3.Distance(ally.Position, player.Position) < 3.5f)
                {
                    nextState = AllyState.NoAction;
                    vectorToTarget = Vector3.zero;
                }
                else
                {
                    vectorToTarget = VectorFromAngle(Direction(ally.Position, player.Position));
                }
            }
            else
            {
                vectorToTarget = VectorFromAngle(Direction(ally.Position, enemy.Position));
            }

            ally.setState(nextState, vectorToTarget);
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
