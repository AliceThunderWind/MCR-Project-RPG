using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mediator
{
    public class Mediator : MonoBehaviour
    {
        protected CommandDispatcher command = CommandDispatcher.Instance;
        private List<Character> enemies = new List<Character>();

        private void Awake()
        {
            command.Subscribe<RegisterEnemyCommand>(OnRegisterEnemyCommand);
        }

        private void OnRegisterEnemyCommand(RegisterEnemyCommand cmd)
        {
            enemies.Add(cmd.who);
        }

        [SerializeField] private Player player;

        public void ArcherBehaviour(Archer enemy)
        {
            if (enemy.IsHit || enemy.CharacterState == CharacterState.Dead)
            {
                enemy.setState(ArcherState.NoAction, 0);
                return;
            }

            float directionToTarget;
            Vector2 targetPosition;
            ArcherState nextState;
            if (enemy.GetConfuse())
            {
                Character closestEnemy = getClosestEnemy(enemy);
                if(closestEnemy == null)
                {
                    enemy.setState(ArcherState.WalkRandom, 0);
                    return;
                }
                targetPosition = closestEnemy.transform.position;
            }
            else
            {
                targetPosition = player.transform.position;
            }
            nextState = DecideArcherState(enemy, targetPosition);
            directionToTarget = direction(enemy.Position, targetPosition);
            if (nextState == ArcherState.BackToPos)
            {
                directionToTarget = direction(enemy.Position, enemy.InitialPosition);
            }
            enemy.setState(nextState, directionToTarget);
            enemy.setTargetPosition(targetPosition);
        }

        public void WarriorBehaviour(Warrior enemy)
        {
            if (enemy.IsHit || enemy.CharacterState == CharacterState.Dead)
            {
                enemy.setState(WarriorState.NoAction, 0);
                return;
            }

            float distanceToTarget, directionToTarget;
            WarriorState nextState;
            if (enemy.GetConfuse())
            {
                Character closestEnemy = getClosestEnemy(enemy);
                if(closestEnemy == null)
                {
                    enemy.setState(WarriorState.WalkRandom, 0);
                    return;
                }
                distanceToTarget = Vector3.Distance(closestEnemy.Position, enemy.Position);
                directionToTarget = direction(enemy.Position, closestEnemy.Position);
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
            else if(enemy.GetIsSentry())
            {
                if (Vector2.Distance(enemy.transform.position, enemy.InitialPosition) > 0.5)
                {
                    return WarriorState.BackToPos;
                }
                else
                {
                    return WarriorState.NoAction;
                }
            }
            return WarriorState.WalkRandom;
        }

        private ArcherState DecideArcherState(Archer enemy, Vector2 targetPosition)
        {
            float distanceToTarget = Vector2.Distance(targetPosition, enemy.transform.position);
            if (distanceToTarget < enemy.GetFightDistance)
            {
                return ArcherState.BowAttack;
            }
            else if (distanceToTarget < enemy.GetVisibility)
            {
                return ArcherState.Chase;
            }
            else if (enemy.GetIsSentry())
            {
                if (Vector2.Distance(enemy.transform.position, enemy.InitialPosition) > 0.5)
                {
                    return ArcherState.BackToPos;
                }
                else
                {
                    return ArcherState.NoAction;
                }
            }
            return ArcherState.WalkRandom;
        }



        private float direction(Vector3 from, Vector3 to)
        {
            float dir = Vector3.Angle(to - from, Vector3.right);
            if (to.y < from.y) dir *= -1;
            return dir;
        }

        private Character getClosestEnemy(Character source)
        {
            Character closestEnemy = null;
            foreach(Character c in enemies)
            {
                if((closestEnemy == null || 
                    Vector2.Distance(c.transform.position, source.transform.position) < Vector2.Distance(closestEnemy.transform.position, source.transform.position) 
                    ) && c != source)
                {
                    closestEnemy = c;
                }
            }
            return closestEnemy;
        }
    }
}
