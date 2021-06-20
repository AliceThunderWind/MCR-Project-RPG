using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHPHandler : MonoBehaviour
{
    private CommandDispatcher mediator = CommandDispatcher.Instance;

    private float health;

    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite femptyHeart;


    // Start is called before the first frame update
    void Start()
    {
        InitHealth();
    }

    public void InitHealth()
    {
        this.health = 100f;
        displayHealth();
    }

    /// <summary>
    /// Méthode permettant d'afficher les points de vie avec le nombre de coeurs correspondants
    /// </summary>
    private void displayHealth()
    {
        GetComponentInChildren<Text>().text = this.health.ToString("0");
        float units = this.health / 20;
        for (int i = 0; i < 5; ++i)
            hearts[i].gameObject.SetActive(false);

        for (int i = 0; i < units; ++i)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    /// <summary>
    /// Méthode permettant de set le nombre de points de vie et d'actualiser l'affichage
    /// </summary>
    /// <param name="hp"></param>
    public void setHp(float hp)
    {
        this.health = hp;
        displayHealth();
    }

}
