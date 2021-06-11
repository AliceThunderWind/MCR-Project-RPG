using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mediator
{
    class Mediator : MonoBehaviour
    {
        protected CommandDispatcher command = CommandDispatcher.Instance;

        [SerializeField] private Player player;

        public void WarriorBehaviour(Warrior enemy)
        {
            if (enemy.IsHit || enemy.CharacterState == CharacterState.Dead)
            {
                enemy.setState(WarriorState.NoAction, 0);
                return;
            }

            float distanceToTarget, directionToTarget;
            WarriorState nextState;
            if (enemy.Confuse)
            {
                AskClosestEnemyCommand cmd = new AskClosestEnemyCommand();
                cmd.source = enemy;
                command.Publish(cmd);
                if (enemy.ClosestEnemy == null) return;
                distanceToTarget = Vector3.Distance(enemy.ClosestEnemy.Position, enemy.Position);
                directionToTarget = direction(enemy.Position, enemy.ClosestEnemy.Position);
            }
            else
            {
                distanceToTarget = Vector3.Distance(player.Position, enemy.Position);
                directionToTarget = direction(enemy.Position, player.Position);
            }
            nextState = DecideWarriorState(enemy, distanceToTarget);
            if (nextState == WarriorState.BackToPos) {
                directionToTarget = direction(enemy.Position, enemy.InitialPosition);
            }
            enemy.setState(nextState, directionToTarget);
            
        }

        private WarriorState DecideWarriorState(Warrior enemy, float distanceToTarget)
        {
            if (distanceToTarget < enemy.GetFightDistance)
            {
                return WarriorState.MeleeAttack;
            }
            else if (distanceToTarget < enemy.GetVisibility)
            {
                return WarriorState.Chase;
            }
            else
            {
                if (enemy.IsSentry)
                {
                    return WarriorState.BackToPos;
                }
            }
            return WarriorState.WalkRandom;
        }



        private float direction(Vector3 from, Vector3 to)
        {
            float dir = Vector3.Angle(to - from, Vector3.right);
            if (to.y < from.y) dir *= -1;
            return dir;
        }
    }
}
