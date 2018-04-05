using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour {
    public static float prehappiness = 5;
    /*
    public GameObject predancehappiness;
    void Start()
    {
        if (predancehappiness == null) predancehappiness = GameObject.Find("predancehappiness"); // search slider in this object if its not set in unity inspector
        Debug.Log(predancehappiness.GetComponent<Slider>().value.ToString());
    }
*/
    public void PlayGame ()
    {
        prehappiness = (GameObject.Find("prehappiness").GetComponent<Slider>().value);
        //prehappiness = (predancehappiness);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Play button pressed");
    }

    public void QuitGame()
    {
        Debug.Log("quit Game");
        Application.Quit();
    }
}   
