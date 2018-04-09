using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiSpawner : MonoBehaviour {

	public GameObject confetti;

	private Vector3 leftPos;
	private Vector3 rightPos;
	private Vector3 middlePos;
	private Quaternion q;
	private Quaternion q2;
	private Quaternion q3;

	// Use this for initialization
	void Start () {
		leftPos = new Vector3 (-27.77f, 5.255511f, 50.38f);
		rightPos = new Vector3 (-19.69f, 5.255511f, 58.03f);
		middlePos = new Vector3 (-22.67f, 8.23f, 53.80f);
		q = new Quaternion ();
		q2 = new Quaternion ();
		q3 = new Quaternion ();
		q.eulerAngles = new Vector3 (-70f, -180f, 0f);
		q2.eulerAngles = new Vector3 (-70f, -270f, 0f);
		q3.eulerAngles = new Vector3 (-90f, -105.7f, 0f);


		//InvokeRepeating ("SpawnConfettiLeft", 0f, 4f);
		//InvokeRepeating ("SpawnConfettiRight", 2f, 4f);
	}

	public void SpawnConfettiMiddle(){
		Instantiate(confetti, middlePos, q3);
	}

	public void SpawnConfettiLeft(){

		Instantiate(confetti, leftPos, q);
	}

	public void SpawnConfettiRight(){
		Instantiate(confetti, rightPos, q2);
	}
}
