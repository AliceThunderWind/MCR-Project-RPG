using Assets.Scripts.Mediator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Character.Selector
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] private GameMediator mediator;
        [SerializeField] private Player[] warriors;
        [SerializeField] private Player[] archers;
        [SerializeField] private Player[] wizzards;

        [SerializeField] private int selected = 0;

        public void Start()
        {

            selected = (int)mediator.PlayerLevel;
            Debug.Log("PlayerSelector");
            Debug.Log(mediator.PlayerLevel);
            Debug.Log(mediator.PlayerClass);
            ChooseClick();

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

        public void changeWeapon(int direction)
        {
            Debug.Log("changingWeapon");
            if (direction < 0) PreviousClick();
            if (direction > 0) NextClick();
            ChooseClick();
        }

        private void ChooseClick()
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
            
        }

        private void PreviousClick()
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

        public void NextClick()
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
