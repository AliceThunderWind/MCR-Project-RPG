using System;
using UnityEngine;

namespace Assets.Scripts.Hit
{
    class CommandMediator : MonoBehaviour
    {
        private global::Mediator mediator = global::Mediator.Instance;

        void Start()
        {
            mediator.Subscribe<HitCharacterCommand>(OnHitPlayer);
            mediator.Subscribe<HitObjectCommand>(OnHitObject);
            mediator.Subscribe<HpIncreaseCommand>(OnHpIncrease);
            mediator.Subscribe<HpDecreaseCommand>(OnHpDecrease);
        }

        private void OnHitObject(HitObjectCommand cmd)
        {
            cmd.What.GetComponent<IBrakeable>().brake();
        }

        private void OnHpIncrease(HpIncreaseCommand cmd)
        {
            cmd.What.GetComponent<IFighter>().heal(cmd.Hp);
        }

        private void OnHitPlayer(HitCharacterCommand cmd)
        {
            cmd.What.GetComponent<IFighter>().damage(cmd.Damage);
        }


        private void OnHpDecrease(HpDecreaseCommand cmd)
        {
            cmd.What.GetComponent<IFighter>().damage(cmd.Hp);
        }

    }
}