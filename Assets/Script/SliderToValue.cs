using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToValue : MonoBehaviour {

	public Text sliderValue;
	public Slider slider;
	
	public string text = "";
 
 	void Update()
	{
		text = slider.value.ToString();
		sliderValue.text = text;
 	}
}
