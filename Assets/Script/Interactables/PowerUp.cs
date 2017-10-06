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
*/

public class PowerUp : Interactable
{
    public float boostPercent;
    public float boostDuration;

    private bool boostCollected;
    private float timeRemaining;

    // Use this for initialization
    protected override void Start()
    {
        boostCollected = false;
        timeRemaining = boostDuration;
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
                removePowerUp();
            }
        }
    }

    // Run base class code and signal that the PowerUp has been collected
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        boostCollected = true;
    }

    // Function that calcutes the boosted value of the PowerUp's attribute
    protected float calculateNewValue(float original, float percentIncrease)
    {
        return original + (original * (percentIncrease / 100));
    }

    // Deletes the game object from the scence
    protected virtual void removePowerUp()
    {
        Destroy(gameObject);
    }
}