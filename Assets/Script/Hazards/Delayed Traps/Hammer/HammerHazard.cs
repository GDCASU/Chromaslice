using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Paul Gellai 10/31/17 - main script for Hammer Hazard 

public class HammerHazard : MonoBehaviour {


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
        // This method checks to see if the projectile has been destroyed or not - is destroyed in HazardHelper script
        if(projectile == null)
        {
            // The below code ensures that the hazard stays in existence for the amount of time trapDuration is set to.
            if(!started)
            {
                startTime = Time.time;
                started = true;
            }
            if (Time.time - trapDuration >= startTime)
            {
                DestroyObject(gameObject);
            }
        }
	}
}
