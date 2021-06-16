using Assets.Scripts.Character.Selector;
using Assets.Scripts.Hit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mediator
{
    public sealed class GameMediator : MonoBehaviour
    {
        private CommandDispatcher command = CommandDispatcher.Instance;

        private List<Enemy> enemies = new List<Enemy>();
        [SerializeField] private Player player;
        [SerializeField] private GUIHPHandler GUIhp;
        [SerializeField] private PlayerSelector PlayerSelector;
        [SerializeField] private GameObject PlayersContainer;
        [SerializeField] private CameraMovement MainCamera;


        public Vector3 PlayerPosition { get { return player.Position; } }
        private void Awake()
        {
            command.Subscribe<HitCharacterCommand>(OnHitCharacter);
            command.Subscribe<HitBreakeableCommand>(OnHitBreakable);
            command.Subscribe<HpIncreaseCommand>(OnHpIncrease);
            command.Subscribe<HpDecreaseCommand>(OnHpDecrease);
            command.Subscribe<KnockbackCommand>(OnKnockback);
        }

        internal void SelectPlayer(Player player)
        {
            this.player = player;
            player.Selected = true;
            MainCamera.SetTarget(this.player.transform);
            PlayerSelector.gameObject.SetActive(false);
            GUIhp.gameObject.SetActive(true);
        }

        public void Start()
        {
            if(player == null)
            {
                MainCamera.SetTarget(PlayersContainer.transform);
                GUIhp.gameObject.SetActive(false);
                PlayerSelector.gameObject.SetActive(true);
            }else{
                player.Selected = true;
                MainCamera.SetTarget(player.transform);
                GUIhp.gameObject.SetActive(true);
                PlayerSelector.gameObject.SetActive(false);
            }
        }

        public void PlayerChangeHp(float newHp)
        {
            GUIhp.setHp(newHp);
        }

        public void registerEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void unregisterEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        private void OnHitBreakable(HitBreakeableCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().Brake();
        }

        private void OnHitCharacter(HitCharacterCommand cmd)
        {
            ICharacter character = cmd.What.GetComponent<ICharacter>();
            if (character != null)
            {
                character.Damage(cmd.Damage);
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
            
            if (enemy.GetConfuse)
                toAttack = FindClosestEnemy(enemy.GetPlayerPosition); 

            distanceToTarget = Vector3.Distance(enemy.Position, toAttack.Position);            
            nextState = DecideEnemyState(enemy, distanceToTarget);
            
            if(nextState == EnemyState.BackToPos)
            {
                vectorToTarget = VectorFromAngle(Direction(enemy.Position, enemy.InitialPosition));
            }
            else
            {
                vectorToTarget = VectorFromAngle(Direction(enemy.Position, toAttack.Position));
            }

            enemy.setState(nextState, vectorToTarget);
            
        }

        private EnemyState DecideEnemyState(Enemy enemy, float distanceToTarget)
        {
            if (distanceToTarget < enemy.GetFightDistance)
            {
                return EnemyState.Attack;
            }
            else if (distanceToTarget < enemy.GetVisibility)
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

        private Enemy FindClosestEnemy(Vector3 from)
        {
            Enemy closestEnemy = enemies[0];
            float minDistance = float.MaxValue;
            for (int i = 1; i < enemies.Count; i++)
            {
                float enemyDistance = Vector3.Distance(from, enemies[i].GetPlayerPosition);
                if (enemyDistance < minDistance)
                {
                    minDistance = enemyDistance;
                    closestEnemy = enemies[i];

                }
            }
            return closestEnemy;
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
