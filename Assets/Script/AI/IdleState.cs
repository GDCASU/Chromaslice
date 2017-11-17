using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class is the base/default state for the AI.
 *                  I have provided a swapTimer to demonstrate how evaluating conditions 
 *                  swaps between the different states. 
 * Version:         1.0.0
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 * 
 * 
 * Description:     Updated the State to accomodate the new Transitioning system
 * Version:         1.1.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */

// Create the Scriptable Object
[CreateAssetMenu (menuName = "AI/States/Idle")]

public class IdleState : State
{
    public override void Enter(StateMachine sm)
    {
        Debug.Log("Entering State: Idle");

        // Add the methods for evaluating conditions to a list
        Conditions = new List<Func<bool>>() { Condition1, Condition2, Condition3 };
        // Set the data required for creating the transitions between the states
        StateTransitions.CreateStateTransitions(this, sm.AllStates, Conditions);
    }

    public override void Execute(StateMachine sm)
    {
        
    }

    public override void Exit(StateMachine sm)
    {
        Debug.Log("Exiting State: Idle");
    }

    // Condition for transitioning to IDLE STATE
    protected override bool Condition1()
    {
        Debug.Log("Evaluating Condition 1");
        return true;
    }

    // Condition for transitioning to ATTACK STATE
    protected override bool Condition2()
    {
        Debug.Log("Evaluating Condition 2");
        return true;
    }
    
    // Condition for transitioning to POWERUP STATE
    protected override bool Condition3()
    {
        Debug.Log("Evaluating Condition 3");
        return true;
    }
}