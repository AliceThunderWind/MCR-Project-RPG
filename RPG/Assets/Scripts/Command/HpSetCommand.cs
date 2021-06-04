using Assets.Scripts.Command;

public class HpSetCommand : ICommand
{
   public float Hp { 
        get; 
        internal set; 
    }
}
