using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{

    private string selectedCharacterDataName = "CharacterClass";
    private string selectedCharacterLevel = "CharacterLevel";

    int PlayerLevel;
    int PlayerClass;

    public void Start()
    {
        PlayerLevel = PlayerPrefs.GetInt(selectedCharacterLevel, 0);
        PlayerClass = PlayerPrefs.GetInt(selectedCharacterDataName, 0);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void retry()
    {

        PlayerPrefs.SetInt(selectedCharacterLevel, PlayerLevel);
        PlayerPrefs.SetInt(selectedCharacterDataName, PlayerClass);
        SceneManager.LoadScene(PlayerLevel + 1);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
