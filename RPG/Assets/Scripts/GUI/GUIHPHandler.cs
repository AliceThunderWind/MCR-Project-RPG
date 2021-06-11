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
        mediator.Subscribe<HpDisplayCommand>(OnHpDisplay);
        InitHealth();
    }

    public void InitHealth()
    {
        this.health = 100f;
        displayHealth();
    }

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

    void OnHpDisplay(HpDisplayCommand command)
    {
        this.health = command.Hp;
        displayHealth();
    }

}
