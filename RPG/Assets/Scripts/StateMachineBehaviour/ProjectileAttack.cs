using Assets.Scripts.Characters;
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe qui permet de créer des projectiles
/// </summary>
public abstract class ProjectileAttack : StateMachineBehaviour
{
    [SerializeField] public Character target;
    [SerializeField] public Character source;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int burst;
    [SerializeField] private float velocity;  

    /// <summary>
    /// Méthode permettant d'instancier un projectile
    /// </summary>
    /// <param name="targetDirection"></param>
    /// <param name="temp"></param>
    /// <returns></returns>
    protected IEnumerator LaunchProjectile(Vector3 targetDirection, Vector2 temp)
    {
        for (int i = 0; i < burst; ++i)
        {

            Vector3 projectilePosition = source.Position + targetDirection.normalized * 2;
            projectilePosition.z = -1;
            GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
            float rotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            targetDirection = targetDirection.normalized;
            Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
            projectileBody.velocity = temp.normalized * velocity;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            yield return new WaitForSeconds(0.1f);
        }
    }
}