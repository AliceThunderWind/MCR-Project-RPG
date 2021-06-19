using Assets.Scripts.Mediator;

namespace Assets.Scripts.Hit
{
    public interface ICharacter
    {
        float Damage(float damage);
        float heal(float hp);
        GameMediator getMediator();
    }
}
