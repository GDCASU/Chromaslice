using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class serves as a speed powerup for the team that picks it up
 * The increased speed is calculated using a percentage increase of the original
 * specified by a public field in the base script.
 * It also inherits from the PowerUp class
 * 
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 15, 2017
 * 
 * Version 1.0.1
 * Author: Zachary Schmalz
 * Date: September 22, 2017
 * Revisions: Redefined the component where the player's maxSpeed is stored from PlayerInput to PlayerController
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 * Revisions: Added functionality to work with PowerUp Spawners
*/

public class SpeedPowerUp : PowerUp
{
    private float boostPercent;
    private float player1Speed;
    private float player2Speed;

    // Assign PowerUp data
    public override void SetData(float delay = 0, float duration = 0, float percent = 0)
    {
        spawnDelay = delay;
        activeLength = duration;
        boostPercent = percent;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        player1Speed = other.GetComponentInParent<Team>().player1.GetComponent<PlayerController>().MaxSpeed;
        player2Speed = other.GetComponentInParent<Team>().player2.GetComponent<PlayerController>().MaxSpeed;
        base.OnTriggerEnter(other);
    }

    // Update player speed with new values
    public override void Activate()
    {
        Player1.GetComponent<PlayerController>().MaxSpeed = CalculateNewValue(player1Speed, boostPercent);
        Player2.GetComponent<PlayerController>().MaxSpeed = CalculateNewValue(player2Speed, boostPercent);
        base.Activate();
    }

    // Restores the original speed when the boost expires
    // and destroy the gameObject containing this script
    public override void RemovePowerUp()
    {
        Player1.GetComponent<PlayerController>().MaxSpeed = player1Speed;
        Player2.GetComponent<PlayerController>().MaxSpeed = player2Speed;
        base.RemovePowerUp();
    }
}