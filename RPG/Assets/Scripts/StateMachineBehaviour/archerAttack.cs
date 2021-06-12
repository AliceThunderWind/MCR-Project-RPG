using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerAttack : StateMachineBehaviour
{
    public Vector3 target;
    public Archer source;
    public GameObject arrowPrefab;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float arrowSpeed = 20+.0f;
        Vector3 targetDirection = target - source.transform.position;
        Vector2 temp = new Vector2(targetDirection.x, targetDirection.y);

        GameObject arrow = Instantiate(arrowPrefab, source.transform.position + targetDirection.normalized * 2, Quaternion.identity);

        float rotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        targetDirection = targetDirection.normalized;
        Rigidbody2D arrowBody = arrow.GetComponent<Rigidbody2D>();
        arrowBody.velocity = temp.normalized * arrowSpeed;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        source.startAttackCooldown();
    }
}
