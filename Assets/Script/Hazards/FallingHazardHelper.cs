using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazardHelper : MonoBehaviour {

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
        plane.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
        plane.SetActive(true);
        for (int i = 0; i < plane.transform.childCount; i++)
        {
            plane.transform.GetChild(i).gameObject.SetActive(true);
        }
        DestroyObject(gameObject);
    }
}
