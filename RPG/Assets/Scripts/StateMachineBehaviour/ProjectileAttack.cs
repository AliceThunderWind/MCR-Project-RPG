using Assets.Scripts.Characters;
using System.Collections;
using UnityEngine;

public abstract class ProjectileAttack : StateMachineBehaviour
{

    [SerializeField] public Character target;
    [SerializeField] public Character source;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int burst;
    [SerializeField] private float velocity;  

    protected IEnumerator LaunchArrow(Vector3 targetDirection, Vector2 temp)
    {
        for (int i = 0; i < burst; ++i)
        {
            GameObject arrow = Instantiate(projectilePrefab, source.Position + targetDirection.normalized * 2, Quaternion.identity);
            float rotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            targetDirection = targetDirection.normalized;
            Rigidbody2D arrowBody = arrow.GetComponent<Rigidbody2D>();
            arrowBody.velocity = temp.normalized * velocity;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            yield return new WaitForSeconds(0.1f);
        }
    }
}