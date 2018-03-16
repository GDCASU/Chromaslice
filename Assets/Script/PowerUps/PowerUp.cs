using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class serves as the base class for all Power-Ups in the games
 * It is generally used for temporary boosts of player attributes such as speed, maxRopeLength, etc.
 * It also inherits from Interactable
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 15, 2017
 * 
 * Version 1.1.0
 * Author: Zachary Schmalz
 * Date: September 27, 2017
 * Revisions: Added a boost multiplier field. Replaced collision with PowerUp with the method OnPowerUpCollect
 * which is called when the team's rope is wrapped around the PowerUp and breaks around it.
 * 
 * Version 1.1.1
 * Author: Zachary Schmalz
 * Date: October 4, 2017
 * Revisions: Added an Activate function so that PowerUp's can be activated with a button/key
 * 
 * Version 1.1.2
 * Author: Zachary Schmalz
 * Date: October 27, 2017
 * Revisions: Added functionality to allow designers to collect powerUps with a collision instead
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 * Revisions: Added functionality to work with PowerUpSpawners and optimized code
 */

public class PowerUp : Interactable
{
    [HideInInspector] public bool isActive;
    protected float spawnDelay;
    protected float activeLength;

    private float spawnTimer;
    private float timeRemaining;

    protected override void Start()
    {
        isActive = false;
        timeRemaining = activeLength;
        spawnTimer = spawnDelay;

        // Hide object on start
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // First calculate the spawn delay, then make the power-up visisble, then count down activationg length when activated
    protected override void Update()
    {
        // Subtract from the spawn delay
        if (spawnTimer > .1f)
        {
            spawnTimer -= Time.deltaTime;
        }

        // Delay timer is over, activate the components
        else if (spawnTimer <= .1f && spawnTimer > 0)
        {
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            spawnTimer = -1;
        }

        // Subtract from time remaining on power-up when activated
        else
        {
            if (isActive)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }

                else
                {
                    RemovePowerUp();
                }
            }
        }
    }

    // When triggered
    protected void OnTriggerEnter(Collider other)
    {
        Team = other.GetComponentInParent<Team>();
        Team.CurrentPowerUp = gameObject;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Activate the powerUp
    public virtual void Activate()
    {
        Debug.Log("PowerUp Activated");
        if (isActive != true)
        {
            isActive = true;
        }
    }

    // Assign power-up data. 3 parameters is provided to pass in all required data for the powerup.
    // Some use all of them, some do not, and all of the current power-ups ovveride this method.
    public virtual void SetData(float param1 = 0, float param2 = 0, float param3 = 0)
    {
        spawnDelay = param1;
        activeLength = param2;
    }

    // Function that calcutes the boosted value of the PowerUp's attribute
    protected float CalculateNewValue(float original, float percentIncrease)
    {
        return original + ((original * percentIncrease) / 100);
    }

    // Deletes the game object from the scence
    public virtual void RemovePowerUp()
    {
        // If the spawner is not continuous, delete the spawner
        if (GetComponentInParent<PowerUpSpawner>())
            if (GetComponentInParent<PowerUpSpawner>().continuousSpawn == false)
                Destroy(transform.parent.gameObject);

        Destroy(gameObject);
        Team.CurrentPowerUp = null;
    }
}