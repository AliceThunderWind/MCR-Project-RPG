using System;
using UnityEngine;

namespace Assets.Scripts.Hit
{
    class CommandMediator : MonoBehaviour
    {
        private global::Mediator mediator = global::Mediator.Instance;

        void Start()
        {
            mediator.Subscribe<HitCharacterCommand>(OnHitCharacter);
            mediator.Subscribe<HitBreakeableCommand>(OnHitBreakable);
            mediator.Subscribe<HpIncreaseCommand>(OnHpIncrease);
            mediator.Subscribe<HpDecreaseCommand>(OnHpDecrease);
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
            cmd.What.GetComponent<ICharacter>().damage(cmd.Damage);
        }


        private void OnHpDecrease(HpDecreaseCommand cmd)
        {
            cmd.What.GetComponent<ICharacter>().damage(cmd.Hp);
        }

    }
}