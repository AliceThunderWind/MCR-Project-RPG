﻿using Assets.Scripts.Mediator;
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

        private string selectedCharacterDataName = "SelectedCharacter";

        public void Start()
        {
            selected = PlayerPrefs.GetInt(selectedCharacterDataName, 0);
            players[selected].gameObject.SetActive(true);
            //ChoseClick();
            
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
            PlayerPrefs.SetInt(selectedCharacterDataName, selected);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        

    }
}
