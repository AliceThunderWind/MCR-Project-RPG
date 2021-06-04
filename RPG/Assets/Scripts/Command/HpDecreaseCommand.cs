using Assets.Scripts.Command;

public class HpDecreaseCommand : ICommand
{
    public float Hp
    {
        get;
        internal set;
    }
}
