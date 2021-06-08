using Assets.Scripts.Command;
using Assets.Scripts.Hit;
using UnityEngine;

public class HpDecreaseCommand : ICommand
{
    public float Hp
    {
        get;
        internal set;
    }
    public Collider2D What { get; internal set; }
}
