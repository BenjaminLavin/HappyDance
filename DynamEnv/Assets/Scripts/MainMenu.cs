using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour {
    public static float prehappiness = 5;

	public Slider preHappinessSlider;

	public void HappinessChanged(){
		prehappiness = preHappinessSlider.value;
	}

    public void PlayGame ()
    {
		SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}   
