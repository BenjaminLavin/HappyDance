using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AvatarMover : MonoBehaviour {

    private GameObject character;
    private GameObject character2;
    private GameObject character3;


    private float x = 0;

	// Use this for initialization
	void Start () {
        character =  Instantiate(Resources.Load(@"C:\Users\Justin\Documents\Avatar Scripting\Assets\UCharCtrlBack.prefab")) as GameObject;
        character2 = GameObject.Find("UCharCtrlBack");


	}
	
	// Update is called once per frame
	void Update () {
        character.transform.Rotate(x, x, x);
        character2.transform.Rotate(x, x, x);
        x = x+10;
		
	}
}
