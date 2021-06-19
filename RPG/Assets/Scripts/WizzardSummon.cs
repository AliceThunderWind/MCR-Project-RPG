using System.Collections;
using Assets.Scripts.Wizzard;

namespace Assets.Scripts
{
    class WizzardSummon : Ally
    {
        public WizzardCreatureSlot mySlot { get; set; }
        protected override IEnumerator DieCo()
        {
            if (mySlot) mySlot.Empty = true;
            yield return base.DieCo();
        }
    }
 }
