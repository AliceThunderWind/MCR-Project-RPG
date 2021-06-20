using Assets.Scripts.Mediator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Character.Selector
{
    /// <summary>
    /// Objet permettant de choisir la classe du joueur
    /// </summary>
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] private GameMediator mediator;
        [SerializeField] private Player[] warriors;
        [SerializeField] private Player[] archers;
        [SerializeField] private Player[] wizzards;

        [SerializeField] private int selected;

        string[,] weapons = new string[3, 3] {{"sword","hammer","axe"},
                                         {"bow","dagger","crossbow"},
                                         {"fire","confusion","summon"}};
        /// <summary>
        /// Appelé une seule fois, au début par le GameMediator
        /// </summary>
        public void StartOnce()
        {

            selected = (int)mediator.PlayerLevel;
            Choose();

            switch (mediator.PlayerClass)
            {
                case GameMediator.CharacterClass.Warrior:
                    warriors[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Archer:
                    archers[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Wizzard:
                    wizzards[selected].gameObject.SetActive(true);
                    break;
            }           
           
        }

        /// <summary>
        /// Retourne le nom de l'arme du joueur
        /// </summary>
        /// <returns>Le nom</returns>
        public string PlayerWeaponName()
        {
            return weapons[(int)mediator.PlayerClass, selected];
        }

        /// <summary>
        /// Méthode permettant de changer d'arme
        /// </summary>
        /// <param name="direction">Direction du changement (gauche, droit)</param>
        /// <returns>Le nom de l'arme</returns>
        public string ChangeWeapon(int direction)
        {
            if (direction < 0) Previous();
            if (direction > 0) Next();
            return Choose();
        }

        /// <summary>
        /// Méthode permettant d'obtenir l'arme correspondant à la bonne classe
        /// </summary>
        /// <returns></returns>
        private string Choose()
        {
            switch (mediator.PlayerClass)
            {
                case GameMediator.CharacterClass.Warrior:
                    mediator.SelectPlayer(warriors[selected]);
                    break;
                case GameMediator.CharacterClass.Archer:
                    mediator.SelectPlayer(archers[selected]);
                    break;
                case GameMediator.CharacterClass.Wizzard:
                    mediator.SelectPlayer(wizzards[selected]);
                    break;
            }

            return PlayerWeaponName();


        }

        /// <summary>
        /// Méthode permettant d'obtenir l'arme précédente, correspondant à la bonne classe
        /// </summary>
        private void Previous()
        {
            int previous = selected--;
            if (selected < 0) selected = (int) mediator.PlayerLevel;
            switch (mediator.PlayerClass)
            {
                case GameMediator.CharacterClass.Warrior:
                    warriors[previous].gameObject.SetActive(false);
                    warriors[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Archer:
                    archers[previous].gameObject.SetActive(false);
                    archers[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Wizzard:
                    wizzards[previous].gameObject.SetActive(false);
                    wizzards[selected].gameObject.SetActive(true);
                    break;
            }           
           
        }

        /// <summary>
        /// Méthode permettant d'obtenir l'arme suivante, correspondant à la bonne classe
        /// </summary>
        public void Next()
        {
            int previous = selected++;
            if (selected > (int) mediator.PlayerLevel) selected = 0;
            switch (mediator.PlayerClass)
            {
                case GameMediator.CharacterClass.Warrior:
                    warriors[previous].gameObject.SetActive(false);
                    warriors[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Archer:
                    archers[previous].gameObject.SetActive(false);
                    archers[selected].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Wizzard:
                    wizzards[previous].gameObject.SetActive(false);
                    wizzards[selected].gameObject.SetActive(true);
                    break;
            }

        }       

    }
}
