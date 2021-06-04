using Assets.Scripts.Command;

public class HpDisplayCommand : ICommand
{
    public float Hp
    {
        get;
        internal set;
    }
}
