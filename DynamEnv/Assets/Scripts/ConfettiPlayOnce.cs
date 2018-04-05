using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiPlayOnce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ParticleSystem>().Play();
		Invoke ("DestroySelf", 2f);
	}

	void DestroySelf(){
		Destroy (this.gameObject);
	}
}
