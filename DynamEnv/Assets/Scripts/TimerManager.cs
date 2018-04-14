using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {


	private Camera mainCam;
	public float danceSeconds = 15f;
    public bool danceDone = false;
    public int dancerNumber = 0;


	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		Invoke ("cameraout", danceSeconds);
	}

	public void cameraout(){
		mainCam.GetComponent<Animator> ().SetTrigger ("cameraout");
		StartCoroutine (FadeOut (mainCam.GetComponent<AudioSource>(), 7f));
		Invoke ("fadeout", 7f);
        danceDone = true;
        dancerNumber++;
	}

	void fadeout(){
		StartCoroutine (GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In,"ExportMenu"));
	}

	public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;

		while (audioSource.volume > 0) {
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}

		audioSource.Stop ();
		audioSource.volume = startVolume;
	}
}
