using Assets.Scripts.Mediator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Character.Selector
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] private GameMediator mediator;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button chooseButton;
        [SerializeField] private Player[] warriors;
        [SerializeField] private Player[] archers;
        [SerializeField] private Player[] wizzards;

        [SerializeField] private int selected = 0;

        public void Awake()
        {
            
            // Must Have EventSystem somewhere in hiearchy
            // (check prefabs) for buttons to work
            previousButton.onClick.AddListener(PreviousClick);
            nextButton.onClick.AddListener(NextClick);
            chooseButton.onClick.AddListener(ChooseClick);

            if (mediator.PlayerLevel == GameMediator.Level.Level1)
                ChooseClick();

           
            switch (mediator.PlayerClass)
            {
                case GameMediator.CharacterClass.Warrior:
                    warriors[0].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Archer:
                    archers[0].gameObject.SetActive(true);
                    break;
                case GameMediator.CharacterClass.Wizzard:
                    wizzards[0].gameObject.SetActive(true);
                    break;
            }
            
           
        }

        private void ChooseClick()
        {
            mediator.PlayerClass = GameMediator.CharacterClass.Wizzard;
            selected = 1;
            wizzards[0].gameObject.SetActive(false);
            wizzards[selected].gameObject.SetActive(true);
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
            if (selected < 0) selected = 0;

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
            if (selected > (int) mediator.PlayerLevel) selected = (int) mediator.PlayerLevel;

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
