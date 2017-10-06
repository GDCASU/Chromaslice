using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text text;
	public float timeLeft;
	// Use this for initialization
	void Start () 
	{
		timeLeft = 180f;	
	}
	
	// Update is called once per frame
	void Update ()
	{
		 timeLeft = timeLeft - Time.deltaTime;
		 int min = Mathf.FloorToInt(timeLeft / 60);
 		 int sec = Mathf.FloorToInt(timeLeft % 60);
 		 text.GetComponent<UnityEngine.UI.Text>().text = min.ToString("00") + ":" + sec.ToString("00");
	}
}
