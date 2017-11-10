using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class serves as a template class for all States in the state machine 
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 */

public abstract class State
{
    protected Team team;
    protected Team enemyTeam;

    // Code to execute when entering the state
    public abstract void Start(StateMachine sm);

    // Code to execute while in the state
    public abstract void Update(StateMachine sm);

    // Code to execute when exiting the state
    public abstract void Exit(StateMachine sm);

    // This is just one idea of how to evaluate which state to go to next
    // Either return the state to go to next, or null if no transition
    public abstract State EvaluateTransitions(StateMachine sm);
}