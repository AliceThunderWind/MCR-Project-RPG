using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Hit
{
    class HitHandler : MonoBehaviour
    {

        private global::CommandDispatcher mediator = global::CommandDispatcher.Instance;

        private List<Character> enemies = new List<Character>();

        private void Awake()
        {
            mediator.Subscribe<HitCharacterCommand>(OnHitCharacter);
            mediator.Subscribe<HitBreakeableCommand>(OnHitBreakable);
            mediator.Subscribe<HpIncreaseCommand>(OnHpIncrease);
            mediator.Subscribe<HpDecreaseCommand>(OnHpDecrease);
            mediator.Subscribe<KnockbackCommand>(OnKnockback);
            mediator.Subscribe<RegisterEnemyCommand>(OnRegisterEnemy);
            mediator.Subscribe<UnregisterEnemyCommand>(OnUnregisterEnemy);
            mediator.Subscribe<AskClosestEnemyCommand>(OnAskClosestEnemy);
        }

        void Start()
        {

        }

        private void OnHitBreakable(HitBreakeableCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().brake();
        }

        private void OnHpIncrease(HpIncreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().heal(cmd.Hp);
        }

        private void OnHitCharacter(HitCharacterCommand cmd)
        {
            ICharacter character = cmd.What.GetComponent<ICharacter>();
            if(character != null)
            {
                character.damage(cmd.Damage);
            }
        }

        private void OnHpDecrease(HpDecreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().damage(cmd.Hp);
        }

        private void OnKnockback(KnockbackCommand cmd)
        {
            cmd.body.AddForce(cmd.force, ForceMode2D.Impulse);
        }

        private void OnRegisterEnemy(RegisterEnemyCommand cmd)
        {
            enemies.Add(cmd.who);
        }

        private void OnUnregisterEnemy(UnregisterEnemyCommand cmd)
        {
            enemies.Remove(cmd.who);
        }

        private void OnAskClosestEnemy(AskClosestEnemyCommand cmd)
        {
            Character closestEnemy = null;
            foreach(Character c in enemies)
            {
                if(c != cmd.source)
                {
                    if ((closestEnemy == null || Vector3.Distance(cmd.source.transform.position, c.transform.position) < Vector3.Distance(cmd.source.transform.position, closestEnemy.transform.position)) && closestEnemy != cmd.source)
                    {
                        closestEnemy = c;
                    }
                }
            }
            cmd.source.ClosestEnemy = closestEnemy;
        }

    }
}