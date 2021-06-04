using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHPHandler : MonoBehaviour
{
    private Mediator mediator = Mediator.Instance;
    // Start is called before the first frame update
    void Start()
    {
        mediator.Subscribe<HpSetCommand>(OnHpSet);
        mediator.Subscribe<HpIncreaseCommand>(OnHpIncrease);
        mediator.Subscribe<HpDecreaseCommand>(OnHpDecrease);
    }


    void OnHpSet(HpSetCommand command)
    {
       GetComponent<Text>().text = command.Hp.ToString("0");
    }

    void OnHpIncrease(HpIncreaseCommand command)
    {
        float newHp = float.Parse(GetComponent<Text>().text) + command.Hp;
        GetComponent<Text>().text = newHp.ToString();
    }

    private void OnHpDecrease(HpDecreaseCommand command)
    {
        float newHp = float.Parse(GetComponent<Text>().text) - command.Hp;
        GetComponent<Text>().text = newHp.ToString();
    }
}
