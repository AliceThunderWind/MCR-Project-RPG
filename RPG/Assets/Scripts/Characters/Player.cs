using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe représentant un joueur
/// </summary>
/// <inheritdoc/>
public class Player : Character, ICharacter
{

    // Start is called before the first frame update

    protected KeyCode nextKey = KeyCode.E;
    protected KeyCode previousKey = KeyCode.Q;

    public bool Selected { get; set; } = false;

    /// <summary>
    /// Méthode permettant de demander le refresh de l'affichage des points de vie
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayHp()
    {
        yield return new WaitForSeconds(.1f);
        mediator.PlayerChangeHp(health);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisplayHp());
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!Selected) return;
        animator.SetBool("moving", false);
        vectorToTarget = Vector3.zero;
        vectorToTarget.x = Input.GetAxisRaw("Horizontal");
        vectorToTarget.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("SwordAttack") && CharacterState != CharacterState.Attack)
            StartCoroutine(AttackCo());

        if (Input.GetKeyDown(nextKey) && CharacterState != CharacterState.Attack)
            mediator.changeWeapon(1, health);

        if (Input.GetKeyDown(previousKey) && CharacterState != CharacterState.Attack)
            mediator.changeWeapon(-1, health);

        if (vectorToTarget != Vector3.zero)
             MoveCharacter(speed);

    }

    /// <summary>
    /// Setter pour la vie
    /// </summary>
    /// <param name="newHealth">Nouvelle valeur</param>
    public void setHealth(float newHealth)
    {
        this.health = newHealth;
    }

    /// <summary>
    /// Méthode permettant d'appliquer des dommages
    /// </summary>
    /// <param name="damage">Nombre de points de dommage</param>
    /// <returns>Nouvelle valeur de la vie</returns>
    public override float Damage(float damage)
    {
        float hp = base.Damage(damage);
        StartCoroutine(DisplayHp());
        return hp;
    }

    /// <summary>
    /// Méthode permettant d'appliquer du soin
    /// </summary>
    /// <param name="heal">Nombre de points de soin</param>
    /// <returns></returns>
    public override float heal(float heal)
    {
        float hp = base.heal(heal);
        StartCoroutine(DisplayHp());
        return hp;
    }

    // static int count = 0;


}
