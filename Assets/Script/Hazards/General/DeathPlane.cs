using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ava Warfield 9/20/17

// Modified by Paul Gellai - This is a simple script that simply ensures that any player that touches an object with this script to it attached dies.

// Developer:   Nick Arnieri
// Date:        1/26/2018
// Description: Changed to inherit from base hazard class

public class DeathPlane : Hazard
{
    /// <summary>
    /// Doesn't call base since DeathPlane doesn't need to use the timer
    /// </summary>
    public override void Start()
    {

    }

    /// <summary>
    /// Doesn't call base since DeathPlane doesn't need to use the timer
    /// </summary>
    public override void Update()
    {
        
    }

    /// <summary>
    /// Kill team that collides
    /// </summary>
    /// <param name="other">Game object colliding with</param>
    public override void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponentInParent<Team>())
            other.transform.parent.GetComponentInParent<Team>().KillTeam();

        else if(other.GetComponent<Rigidbody>())
            Destroy(other);
    }
}
