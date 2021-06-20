using UnityEngine;

namespace Assets.Scripts.StateMachineBehaviour
{
    /// <summary>
    /// Classe qui créé des projectiles au début d'une animation
    /// </summary>
    class ProjectileAttackStateEnter : ProjectileAttack
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 targetDirection = target.Position - source.Position;
            Vector2 temp = new Vector2(targetDirection.x, targetDirection.y);
            source.StartCoroutine(base.LaunchProjectile(targetDirection, temp));
        }
    }
}
