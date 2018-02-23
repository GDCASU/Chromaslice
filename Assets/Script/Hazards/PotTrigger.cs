using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotTrigger : MonoBehaviour {

	private bool inRange;

	// Use this for initialization
	void Start () {
		inRange = false;
	}

	//When players enter the trigger
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			inRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			inRange = false;
		}
	}

	//Return whether player is in range or not
	public bool isInRange(){
		return inRange;
	}
}