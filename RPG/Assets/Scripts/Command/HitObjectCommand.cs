using Assets.Scripts.Command;
using UnityEngine;

public class HitObjectCommand : ICommand
{
    public Collider2D What
    {
        get;
        internal set;
    }

  

}
