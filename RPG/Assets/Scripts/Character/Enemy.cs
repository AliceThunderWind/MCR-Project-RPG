using Assets.Scripts.Characters;
using Assets.Scripts.Hit;
using System;
using System.Collections;
using UnityEngine;

abstract public class Enemy : Character, ICharacter
{

    [SerializeField] protected const float visibility = 5f;
    [SerializeField] protected const float fightDistance = 1.45f;

    
    // for random walk in 5 second periode with 5s pause
    

    // Start is called before the first frame update


    // Update is called once per frame

}
