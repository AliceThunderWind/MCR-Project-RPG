using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// M�thode permettant de quitter le jeu
    /// </summary>
    public void quitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
