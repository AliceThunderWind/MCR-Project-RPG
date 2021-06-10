using Assets.Scripts.Characters;
using Assets.Scripts.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnregisterEnemyCommand : ICommand
{
    public Character who
    {
        get;
        internal set;
    }
}
