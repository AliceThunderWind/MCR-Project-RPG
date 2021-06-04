using Assets.Scripts.Command;
using UnityEngine;

public class PlayerHitCommand : ICommand
{
    public Collider2D What
    {
        get;
        internal set;
    }
}
