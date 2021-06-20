using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipped : MonoBehaviour
{

    [SerializeField] private GameMediator mediator;
    //public TMPro.TextMeshPro stuff;
    string[,] weapons = new string[3,3] { {"sword","hammer","axe"}, 
                                         {"bow","dagger","crossbow"}, 
                                         {"fire","confusion","summon"}};
                                  

    public void changeText(int x, int y)
    {
        Debug.Log("hello");
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = weapons[x, y];
        //stuff.text = weapons[x, y];
    }
}
