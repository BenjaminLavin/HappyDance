using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEManager : MonoBehaviour {

	public GameObject BalloonSpawnerObject;
	public GameObject ConffetiSpawnerObject;
	public GameObject LeftClap;
	public GameObject RightClap;

	private BalloonSpawner bs;
	private ConfettiSpawner cs;
	private Animator lc;
	private Animator rc;
	private int ClapCount;

	void Start(){
		bs = BalloonSpawnerObject.GetComponent<BalloonSpawner> ();
		cs = ConffetiSpawnerObject.GetComponent<ConfettiSpawner> ();
		lc = LeftClap.GetComponent<Animator> ();
		rc = RightClap.GetComponent<Animator> ();
	}


	public void DidMove(DanceMove dm){
		switch (dm) {
		case DanceMove.Clap:
			ClapCount++;
			if (ClapCount % 2 == 0) {
				lc.SetTrigger ("Clap");
			} else {
				rc.SetTrigger ("Clap");
			}
			cs.SpawnConfettiMiddle ();
			break;
		case DanceMove.ArmsLeft:
			bs.SpawnBalloonsLeft ();
			break;
		case DanceMove.ArmsRight:
			bs.SpawnBalloonsRight ();
			break;
		case DanceMove.HandsLeft:
			cs.SpawnConfettiLeft ();
			break;
		case DanceMove.HandsRight:
			cs.SpawnConfettiRight ();
			break;
		default:
			break;
		}
	}
}

public enum DanceMove{
	Clap,
	ArmsLeft,
	ArmsRight,
	HandsLeft,
	HandsRight
}
