using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SexSliderUI : MonoBehaviour {

	public Text male;
	public Text female;

	public float v;

	private Slider slider;

	public void SliderChanged(){
		switch ((int)slider.value) {
		case 0:
			male.color = Color.white;
			female.color = Color.gray;
			break;
		case 1:
			male.color = Color.gray;
			female.color = Color.white;
			break;
		default:
			break;
		};
	}

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
	}
}
