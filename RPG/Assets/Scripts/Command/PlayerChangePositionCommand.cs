using Assets.Scripts.Command;
using UnityEngine;

public class PlayerChangePositionCommand : ICommand
{
    public Vector3 Position
    {
        get;
        internal set;
    }
}
