using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class serves as a rope powerup for the team that picks it up
 * The increased rope length is calculated using a percentage increase of the original
 * specified by a public field in the base script.
 * It also inherits from the PowerUp class
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 15, 2017
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 * Revisions: Added functionality to work with the PowerUp Spawners
*/

public class RopePowerUp : PowerUp
{
    private float maxRopeLength;
    private float boostPercent;

    // Assign power-up data
    public override void SetData(float delay = 0, float duration = 0, float percent = 0)
    {
        spawnDelay = delay;
        activeLength = duration;
        boostPercent = percent;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        maxRopeLength = other.GetComponentInParent<Team>().ropePrefab.GetComponent<Rope>().maxRopeLength;
        base.OnTriggerEnter(other);
    }

    // When triggered, saves the original maxRopeLength field of the rope prefab/currentRope,
    // then calculates the length boost and updates the prefab/currentRope
    public override void Activate()
    {
        Team.ropePrefab.GetComponent<Rope>().maxRopeLength = CalculateNewValue(maxRopeLength, boostPercent);

        // Only update currentRope maxRopeLength if it is not null
        if (Team.currentRope)
        {
            Team.currentRope.GetComponent<Rope>().maxRopeLength = CalculateNewValue(maxRopeLength, boostPercent);
        }
        base.Activate();
    }

    // Restores the original maxRopeLength field of the rope prefab/currentRope
    // and destroy the gameObject containing this script
    public override void RemovePowerUp()
    {
        Team.ropePrefab.GetComponent<Rope>().maxRopeLength = maxRopeLength;
        // Only update currentRope maxRopeLength if it is not null
        if (Team.currentRope)
        {
            Team.currentRope.GetComponent<Rope>().maxRopeLength = maxRopeLength;
        }
        base.RemovePowerUp();
    }
}