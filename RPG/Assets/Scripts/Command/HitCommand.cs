using Assets.Scripts.Command;
using UnityEngine;

public class HitCommand : ICommand
{
    public Collider2D What
    {
        get;
        internal set;
    }

    public float Damage
    {
        get;
        internal set;
    }

}
