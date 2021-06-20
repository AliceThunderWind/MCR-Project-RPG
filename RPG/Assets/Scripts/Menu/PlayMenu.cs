using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    private string selectedCharacterDataName = "CharacterClass";
    private string selectedCharacterLevel = "CharacterLevel";

    private void selectLevel1()
    {
        PlayerPrefs.SetInt(selectedCharacterLevel, (int) GameMediator.Level.Level3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void playAsWarrior()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int) GameMediator.CharacterClass.Warrior);
        selectLevel1();
    }

    public void playAsRanger()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int)GameMediator.CharacterClass.Archer);
        selectLevel1();
    }

    public void playAsWizard()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int)GameMediator.CharacterClass.Wizzard);
        selectLevel1();
    }
}
