using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipped : MonoBehaviour
{                         
    public void changeText(string text)
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }
}
