using Assets.Scripts.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackCommand : ICommand
{
    public Rigidbody2D body
    {
        get;
        internal set;
    }

    public Vector2 force
    {
        get;
        internal set;
    }




}
