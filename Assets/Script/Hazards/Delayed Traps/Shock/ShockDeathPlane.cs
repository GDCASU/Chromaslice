using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Paul Gellai - 10/31/17 - custom death plane script for Shock trap

// Developer:   Nick Arnieri
// Date:        1/26/2017
// Description: Changed to inherit from base hazard class

public class ShockDeathPlane : Hazard
{
    private Renderer rend;

    // Use this for initialization
	public override void Start()
    {
        base.Start();
        rend = gameObject.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	public override void Update()
    {
        base.Update();
	}

    public override void ActivateTrap()
    {
        base.ActivateTrap();
        // Change color to indicate active
        rend.material.color = new Color(255, 0, 255);
    }

    public override void DeactivateTrap()
    {
        base.DeactivateTrap();
        // Change back to normal color
        rend.material.color = new Color(0, 0, 0);
    }

    /// <summary>
    /// Kill team that collides if active
    /// </summary>
    /// <param name="other">Game object colliding with</param>
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        other.transform.parent.GetComponent<Team>().KillTeam();
    }

    /// <summary>
    /// Kill team that collides if active
    /// </summary>
    /// <param name="other">Game object colliding with</param>
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        other.transform.parent.GetComponent<Team>().KillTeam();
    }
}
