using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimationTimer : MonoBehaviour {


	public AvatarController av;
	// Use this for initialization
	void StartDancing(){
		av.shouldStart = true;
	}
}
