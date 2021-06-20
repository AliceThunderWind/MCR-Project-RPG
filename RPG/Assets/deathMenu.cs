using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
    }

    public void retry()
    {

    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
