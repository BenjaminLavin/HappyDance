using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEvents : MonoBehaviour {


    bool clap = false, pointLeft = false, pointRight = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkforEvents();
	}


    void checkforEvents()
    {
        if (clap)
        {
            // Call Dynamic Elements for Clapping

            clap = false;
        }
        if (pointLeft)
        {
            // Call Dynamic Element for Pointing Left

            pointLeft = false;
        }
        if (pointRight)
        {
            // Call Dynamic Element for Pointing Right

            pointRight = false;
        }

    }
}
