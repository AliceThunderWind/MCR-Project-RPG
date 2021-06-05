using UnityEngine;

namespace Assets.Scripts.Hit
{
    class HitMediator : MonoBehaviour
    {
        private Mediator mediator = Mediator.Instance;

        void Start()
        {
            mediator.Subscribe<HitCommand>(OnHit);
        }

        private void OnHit(HitCommand cmd)
        {
            cmd.What.GetComponent<IHittable>().apply(cmd.Damage);
        }
    }
}