using Assets.Scripts.Command;
using UnityEngine;

public class HitBreakeableCommand : ICommand
{
    public Collider2D What
    {
        get;
        internal set;
    }

  

}
