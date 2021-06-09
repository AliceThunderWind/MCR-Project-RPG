using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerAttack : StateMachineBehaviour
{
    public Vector3 target;
    public archer source;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Attack !!!!");
        source.startAttackCooldown();
    }
}
