using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Méthode permettant de quitter le jeu
    /// </summary>
    public void quitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
