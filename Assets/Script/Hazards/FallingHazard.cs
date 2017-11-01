using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour {


    public GameObject projectile;
    public GameObject plane;
    public float trapDuration;
    float startTime = 0;
    bool started = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(projectile == null)
        {
            if(!started)
            {
                startTime = Time.time;
                started = true;
            }
            Debug.Log("Start time: " + startTime);
            Debug.Log("Current time: " + Time.time);
            if (Time.time - trapDuration >= startTime)
            {
                DestroyObject(gameObject);
            }
        }
	}
}
