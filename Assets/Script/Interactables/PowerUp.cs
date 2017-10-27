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
 */

public class PowerUp : Interactable
{
    public bool collisionCollect;
    public float boostPercent;
    public float boostDuration;
    [HideInInspector]public bool isActive;

    protected int boostMultiplier;

    private bool boostCollected;
    private float timeRemaining;

    // Use this for initialization
    protected override void Start()
    {
        boostMultiplier = 1;
        boostCollected = false;
        isActive = false;
        timeRemaining = boostDuration;
        gameObject.GetComponent<Collider>().isTrigger = collisionCollect;
    }

    // When the PowerUp is collected, the timer begins counting down
    // When time expires, the PowerUp is removed
    protected override void Update()
    {
        if (boostCollected)
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

    // If the designers want to use collisions to collect the powerUp
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Team.CurrentPowerUp = gameObject;
        // Note that if the designers choose this route, there would be no boostmultiplier.
        boostMultiplier = 1;
    }

    // Set the Team and hide the object from the scene
    public virtual void OnPowerUpCollect(Team t, int modifier)
    {
        Team = t;
        Team.CurrentPowerUp = gameObject;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if(modifier != 0)
            boostMultiplier = modifier;
    }

    // Trigger the powerUp
    public virtual void Activate()
    {
        Debug.Log("PowerUp Activated");
        if (isActive != true)
        {
            boostCollected = true;
            isActive = true;
        }
    }

    // Function that calcutes the boosted value of the PowerUp's attribute
    protected float CalculateNewValue(float original, float percentIncrease)
    {
        return original + (original * ((boostMultiplier * percentIncrease) / 100));
    }

    // Deletes the game object from the scence
    protected virtual void RemovePowerUp()
    {
        Team.CurrentPowerUp = null;
        Destroy(gameObject);
    }
}