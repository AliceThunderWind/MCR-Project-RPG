using Assets.Scripts.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [SerializeField] private Button choseButton;
        [SerializeField] private Player[] players;
        [SerializeField] private int selected = 0;

        public void Start()
        {
            previousButton.onClick.AddListener(PreviousClick);
            nextButton.onClick.AddListener(NextClick);
            choseButton.onClick.AddListener(ChoseClick);
            players[0].gameObject.SetActive(true);
        }

        private void ChoseClick()
        {
            mediator.SelectPlayer(players[selected]);
        }

        private void PreviousClick()
        {
            players[selected].gameObject.SetActive(false);
            selected--;
            if (selected < 0) selected = players.Length - 1;
            players[selected].gameObject.SetActive(true);
        }

        public void NextClick()
        {
            players[selected].gameObject.SetActive(false);
            selected++;
            if (selected >= players.Length) selected = 0;
            players[selected].gameObject.SetActive(true);
        }

        public void previous()
        {
           
        }

        public void StartGame()
        {
            PlayerPrefs.SetInt("selectedPlayer", selected);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }


    }
}
