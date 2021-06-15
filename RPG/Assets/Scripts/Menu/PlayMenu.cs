using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    private string selectedCharacterDataName = "SelectedCharacter";

    public void playAsWarrior()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void playAsRanger()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void playAsWizard()
    {
        PlayerPrefs.SetInt(selectedCharacterDataName, 5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
