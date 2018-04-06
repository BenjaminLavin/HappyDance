using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour {

	public Image bar;
	public RectTransform button;

	public float value = 0;


	void ValueChange(float newValue){
		float amount = (newValue / 100f) * 180f / 360;
		bar.fillAmount = amount;
		float buttonAngle = amount * 360;
		button.localEulerAngles = new Vector3 (0, 0, -buttonAngle);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ValueChange (value);
	}
}
