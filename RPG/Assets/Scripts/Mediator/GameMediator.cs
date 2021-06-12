using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mediator
{
    public class GameMediator : MonoBehaviour
    {
        protected CommandDispatcher command = CommandDispatcher.Instance;

        [SerializeField] private Player player;

        public Vector3 PlayerPosition { get { return player.Position; } }

        public void EnemyBehaviour(Enemy enemy)
        {
            if (enemy.IsHit || enemy.CharacterState == CharacterState.Dead)
            {
                enemy.setState(EnemyState.NoAction, Vector3.zero);
                return;
            }

            Vector3 vectorToTarget;
            float distanceToTarget;
            Character toAttack = player;
            EnemyState nextState;
            
            if (enemy.GetConfuse)
            {
                AskClosestEnemyCommand cmd = new AskClosestEnemyCommand();
                cmd.source = enemy;
                command.Publish(cmd);
                if (enemy.ClosestEnemy == null) return;
                toAttack = enemy.ClosestEnemy;
            }

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
