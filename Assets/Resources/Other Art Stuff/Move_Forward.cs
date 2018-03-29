using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Forward : MonoBehaviour {

    public float speed = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
