using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Tyler Cole
// Date:        2/19/2018
// Description: Class for the A/C hazard's hitbox

public class HitboxController : MonoBehaviour {

    // bool for when to slow down
    private bool slowDown = false;

    void Update()
    {
        if (slowDown)
        {
            // add slow down method here
        }
    }

    // Behavior for when another objects trigger touches this object
    void OnTriggerEnter(Collider other)
    {
		if(other.CompareTag("Player"))
        {
            slowDown = true;
        }
	}

    // Behavior for when another objects trigger stops touching this object
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            slowDown = false;
        }
    }
}
