using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Paul Gellai - HazardHelper is a script used in all Delayed Traps

public class HazardHelper : MonoBehaviour {

    GameObject projectile;
    GameObject plane;

    // Use this for initialization
    void Start () {
        projectile = gameObject.transform.parent.GetChild(0).gameObject;
        plane = gameObject.transform.parent.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        // All this does: When the projectile hits the ground (or collides with something), it sets the plane (hazard area) to active, as well as its children if it has any.
        // Afterwards, the projectile is destroyed. 
        plane.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
        plane.SetActive(true);
        for (int i = 0; i < plane.transform.childCount; i++)
        {
            plane.transform.GetChild(i).gameObject.SetActive(true);
        }
        DestroyObject(gameObject);
    }
}
