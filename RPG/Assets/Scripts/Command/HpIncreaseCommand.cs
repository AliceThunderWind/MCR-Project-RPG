using Assets.Scripts.Command;

public class HpIncreaseCommand : ICommand
{
    public float Hp
    {
        get;
        internal set;
    }
}
