using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ArcherAttack : StateMachineBehaviour
{

    [SerializeField] public Character target;
    [SerializeField] public Character source;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private int burst;
    [SerializeField] private float velocity;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetDirection = target.Position - source.Position;
        Vector2 temp = new Vector2(targetDirection.x, targetDirection.y);
        source.StartCoroutine(LaunchArrow(targetDirection, temp));

    }

    private IEnumerator LaunchArrow(Vector3 targetDirection, Vector2 temp)
    {
        for (int i = 0; i < burst; ++i)
        {
            GameObject arrow = Instantiate(arrowPrefab, source.Position + targetDirection.normalized * 2, Quaternion.identity);
            float rotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            targetDirection = targetDirection.normalized;
            Rigidbody2D arrowBody = arrow.GetComponent<Rigidbody2D>();
            arrowBody.velocity = temp.normalized * velocity;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            yield return new WaitForSeconds(0.1f);
        }
    }
}