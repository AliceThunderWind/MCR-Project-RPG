using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerAttack : StateMachineBehaviour
{
    public Vector3 target;
    public archer source;
    public GameObject arrowPrefab;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float arrowSpeed = 20.0f;
        Vector3 playerDirection = source.getPlayerPosition() - source.transform.position;
        Vector2 temp = new Vector2(playerDirection.x, playerDirection.y);

        GameObject arrow = Instantiate(arrowPrefab, source.transform.position + playerDirection.normalized, Quaternion.identity);

        float rotation = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        playerDirection = playerDirection.normalized;
        Rigidbody2D arrowBody = arrow.GetComponent<Rigidbody2D>();
        arrowBody.velocity = temp.normalized * arrowSpeed;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        source.startAttackCooldown();
    }
}
