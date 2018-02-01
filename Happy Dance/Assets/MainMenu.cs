using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame ()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("Play button pressed");
    }

    public void QuitGame()
    {
        Debug.Log("quit Game");
        Application.Quit();
    }
}   
