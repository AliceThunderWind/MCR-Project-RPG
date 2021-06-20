using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipped : MonoBehaviour
{                         
    /// <summary>
    /// Méthode permettant de changer le texte du GUI
    /// </summary>
    /// <param name="text">Le nouveau texte</param>
    public void changeText(string text)
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }
}
