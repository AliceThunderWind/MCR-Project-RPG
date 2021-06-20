using Assets.Scripts.Mediator;

namespace Assets.Scripts.Hit
{
    /// <summary>
    /// Interface représentant un personnage
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Applique de dommages
        /// </summary>
        /// <param name="damage">Quantité de dommages</param>
        /// <returns>Vie restante</returns>
        float Damage(float damage);
        /// <summary>
        /// Applique du head
        /// </summary>
        /// <param name="hp">Quantité de heal</param>
        /// <returns>Vie restante</returns>
        float heal(float hp);
        /// <summary>
        /// Getter pour le médiateur
        /// </summary>
        /// <returns>Le médiateur</returns>
        GameMediator getMediator();
    }
}
