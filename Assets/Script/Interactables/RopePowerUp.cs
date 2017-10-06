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
*/

public class RopePowerUp : PowerUp
{

    private float maxRopeLength;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // When triggered, saves the original maxRopeLength field of the rope prefab/currentRope,
    // then calculates the length boost and updates the prefab/currentRope
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        maxRopeLength = team.ropePrefab.GetComponent<Rope>().maxRopeLength;
        team.ropePrefab.GetComponent<Rope>().maxRopeLength = calculateNewValue(maxRopeLength, boostPercent);
        // Only update currentRope maxRopeLength if it is not null
        if (team.currentRope)
        {
            team.currentRope.GetComponent<Rope>().maxRopeLength = calculateNewValue(maxRopeLength, boostPercent);
        }
    }

    // Restores the original maxRopeLength field of the rope prefab/currentRope
    // and destroy the gameObject containing this script
    protected override void removePowerUp()
    {
        team.ropePrefab.GetComponent<Rope>().maxRopeLength = maxRopeLength;
        // Only update currentRope maxRopeLength if it is not null
        if (team.currentRope)
        {
            team.currentRope.GetComponent<Rope>().maxRopeLength = maxRopeLength;
        }
        base.removePowerUp();
    }
}