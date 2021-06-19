using Assets.Scripts.Wizzard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters
{
   

    class PlayerWizzardCreatures : Player
    {
        [SerializeField] private GameObject spawnSpot;
        [SerializeField] private WizzardCreatureSlot[] creaturesSlots;
        [SerializeField] private Ally summon;
        private int SpawnDistance = 3;
      
        protected override IEnumerator AttackCo()
        {
            if (CharacterState != CharacterState.Dead)
            {
                CharacterState = CharacterState.Attack;
                animator.SetBool("attacking", true);


                spawnSpot.transform.position = new Vector3(
                    Position.x + Input.GetAxisRaw("Horizontal") * SpawnDistance,
                    Position.y + Input.GetAxisRaw("Vertical") * SpawnDistance
                );
                
                foreach (WizzardCreatureSlot slot in creaturesSlots)
                {
                    if(slot.Empty)
                    {
                        Ally ally = Instantiate(summon, slot.transform.position, Quaternion.identity);
                        ally.mySlot = slot;
                        ally.Mediator = mediator;
                        slot.Empty = false;
                        mediator.registerAlly(ally);
                    }
                }

                yield return base.AttackCo();
                animator.SetBool("attacking", false);
                yield return new WaitForSeconds(attackCoolDown);
                animator.SetBool("moving", true);
                CharacterState = CharacterState.Idle;
            }
        }

    }
}
