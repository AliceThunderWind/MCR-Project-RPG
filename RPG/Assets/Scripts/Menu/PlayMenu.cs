using Assets.Scripts.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe représentant le comportemnt du GUI de sélection des personnages
/// </summary>
public class PlayMenu : MonoBehaviour
{

    private string selectedCharacterDataName = "CharacterClass";
    private string selectedCharacterLevel = "CharacterLevel";

    /// <summary>
    /// Méthode permettant de lancer le level1
    /// </summary>
    private void selectLevel1()
    {
        PlayerPrefs.SetInt(selectedCharacterLevel, (int) GameMediator.Level.Level1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Méthode permettant de lancer le jeu en tant que classe guerrier
    /// </summary>
    public void playAsWarrior()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int) GameMediator.CharacterClass.Warrior);
        selectLevel1();
    }

    /// <summary>
    /// Méthode permettant de lancer le jeu en tant que classe ranger
    /// </summary>
    public void playAsRanger()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int)GameMediator.CharacterClass.Archer);
        selectLevel1();
    }

    /// <summary>
    /// Méthode permettant de lancer le jeu en tant que classe mage
    /// </summary>
    public void playAsWizard()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, (int)GameMediator.CharacterClass.Wizzard);
        selectLevel1();
    }
}
