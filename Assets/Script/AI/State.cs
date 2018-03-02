using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class serves as a template class for all States in the state machine 
 * Version:         1.0.0
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 * 
 * 
 * Description:     Updated the class to accomodate the transitioning system
 * Version:         1.1.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */

/* @author:         Diego Wilde
 * @date:           Feb 2, 2018
 * @description:    moved priorities directly to the state script
 * 
 **/

public abstract class State : ScriptableObject
{
    // Add the field for setting the StateTransitions ScriptableObject
    // Make sure to add the reference in the inspector for each state
    public StateTransitions StateTransitions;

    public int[] priorities;

    public float AI1DistanceToRopeWeight;
    public float AI2DistanceToRopeWeight;
    public float ropeLengthWeight;
    public float angleWeight;
    public float closestPlayerDistanceToMidpointWeight;

    protected AIBehavior AIBehaviorScript;

    // A list containing the Method Conditions used for evaluating transitions
    protected List<Func<bool>> Conditions;

    protected Team team;
    protected Team enemyTeam;

    // Code to execute when entering the state
    public abstract void Enter(StateMachine sm);

    // Code to execute while in the state
    public abstract void Execute(StateMachine sm);

    // Code to execute when exiting the state
    public abstract void Exit(StateMachine sm);

    // Methods for evaluating conditions for transitioning between states
    // The condition logic MUST relate to the order in which the states are added to
    // AllStates in the StateMachine, i.e. AllStates[0] = IdleState
    // so Condition1 handles the transition from the current state to IdleState.

    protected abstract bool Condition1();   // Condition to transition to IdleState
    protected abstract bool Condition2();   // Condition to transition to AttackState
    protected abstract bool Condition3();   // Condition to transition to PowerUpState
}