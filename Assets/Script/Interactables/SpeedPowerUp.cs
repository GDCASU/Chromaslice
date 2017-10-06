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
 * 
 * Version 1.0.1
 * Author: Zachary Schmalz
 * Date: September 22, 2017
 * Revisions: Redefined the component where the player's maxSpeed is stored from PlayerInput to PlayerController
*/

public class SpeedPowerUp : PowerUp
{
    private float player1Speed;
    private float player2Speed;

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

    // When triggered, saves the original speed of the teams' players,
    // then calculates the speed boost and updates players' speed
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        player1Speed = player1.GetComponent<PlayerController>().maxSpeed;
        player2Speed = player2.GetComponent<PlayerController>().maxSpeed;
        player1.GetComponent<PlayerController>().maxSpeed = calculateNewValue(player1Speed, boostPercent);
        player2.GetComponent<PlayerController>().maxSpeed = calculateNewValue(player2Speed, boostPercent);
    }

    // Restores the original speed when the boost expires
    // and destroy the gameObject containing this script
    protected override void removePowerUp()
    {
        player1.GetComponent<PlayerController>().maxSpeed = player1Speed;
        player2.GetComponent<PlayerController>().maxSpeed = player2Speed;
        base.removePowerUp();
    }
}