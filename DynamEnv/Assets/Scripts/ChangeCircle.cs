using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCircle : MonoBehaviour {

	public Slider slider;
	public Image bar;
	public RectTransform button;

	public float t;
	public float l;

	// Use this for initialization
	void Start () {
		
	}

	void ValueChange(float newValue){
		float amount = (newValue / 100f);
		bar.fillAmount = amount;
		float buttonAngle = amount * 180;
		button.localEulerAngles = new Vector3 (0, 0, -buttonAngle);
	}

	// Update is called once per frame
	void Update () {
		t = slider.value;
		l = Mathf.Lerp (0, 100, Mathf.InverseLerp (1, 9, t));
		ValueChange (l);
	}
}
