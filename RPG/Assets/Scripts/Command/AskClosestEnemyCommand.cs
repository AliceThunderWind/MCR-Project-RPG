using Assets.Scripts.Characters;
using Assets.Scripts.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskClosestEnemyCommand : ICommand
{
    public Character source
    {
        get;
        internal set;
    }
    // Start is called before the first frame update
}
