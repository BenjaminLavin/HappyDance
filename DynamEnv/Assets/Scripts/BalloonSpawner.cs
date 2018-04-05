using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour {

	public GameObject blueBalloon;
	public GameObject greenBalloon;
	public GameObject redBalloon;

	private Vector3 leftPosB;
	private Vector3 rightPosB;
	private Vector3 leftPosG;
	private Vector3 rightPosG;
	private Vector3 leftPosR;
	private Vector3 rightPosR;

	// Use this for initialization
	void Start () {
		leftPosB = new Vector3 (-32.54f, 5.255511f, 45.45f);
		//leftPosG = new Vector3 (-35.72f, 5.255511f, 46.93f);
		leftPosG = new Vector3 (-33.13f, 5.255511f, 45.01f);
		//leftPosR = new Vector3 (-31.53f, 5.255511f, 44.21f);
		leftPosR = new Vector3 (-27.2f, 5.255511f, 41.5f);


		rightPosB = new Vector3 (-13.9f, 5.255511f, 63.2f);
		rightPosG = new Vector3 (-19.7f, 5.255511f, 58f);
		rightPosR = new Vector3 (-7.8f, 5.255511f, 66.8f);

		//InvokeRepeating ("SpawnBalloonsLeft", 1f, 4f);
		//InvokeRepeating ("SpawnBalloonsRight", 3f, 4f);
	}

	public void SpawnBalloonsLeft(){


		//Instantiate(blueBalloon, leftPosB, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseLeft");
		Instantiate(blueBalloon).GetComponent<Animator>().SetTrigger("RiseLeft");
		Instantiate(greenBalloon).GetComponent<Animator>().SetTrigger("RiseLeft");
		Instantiate(redBalloon).GetComponent<Animator>().SetTrigger("RiseLeft");
		//Instantiate(greenBalloon, leftPosG, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseLeft");
		//Instantiate(redBalloon, leftPosR, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseLeft");
	}

	public void SpawnBalloonsRight(){
		Instantiate(blueBalloon, rightPosB, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseRight");
		Instantiate(greenBalloon, rightPosG, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseRight");
		Instantiate(redBalloon, rightPosR, new Quaternion()).GetComponent<Animator>().SetTrigger("RiseRight");
		//Instantiate(redBalloon, rightPosR, new Quaternion());
	}

	// Update is called once per frame
	void Update () {

	}
}
