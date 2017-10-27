using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class is the base/default state for the AI.
 *                  I have provided a swapTimer to demonstrate how evaluating conditions 
 *                  swaps between the different states. 
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 */

public class IdleState : State
{
    private float swapTimer;

    public override void Start(StateMachine sm)
    {
        Debug.Log("Entering State: Idle");
        team = sm.GetTeam();
        swapTimer = 3.0f;
    }

    public override void Update(StateMachine sm)
    {
        Debug.Log("Current State: Idle");
        swapTimer -= Time.deltaTime;
    }

    public override void Exit(StateMachine sm)
    {
        Debug.Log("Exiting State: Idle");
    }

    // Example of how evaluating transitions could work
    public override State EvaluateTransitions(StateMachine sm)
    {
        Debug.Log("Evaluating Transitions...");
        if (swapTimer <= 0)
        {
            return new PowerUpState();
        }

        else
            return null;
    }
}
