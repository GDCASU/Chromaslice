using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class serves as a Finite State Machine for the AI. The default state is Idle and
 *                  provides functionality to swap between the states. Attach this script to the AI team.
 * Version:         1.0.0
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 * 
 * Description:     Updated the class to accomodate the transitioning system
 * Version:         1.1.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */

public class StateMachine : MonoBehaviour
{
    // List that contains all ScriptableObject references for the States
    public List<State> AllStates;
    public State currentState;

    private Team team;
    private Team enemyTeam;

    // Initial state is Idle
    void Start()
    {
        team = gameObject.GetComponent<Team>();
        enemyTeam = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Team>();

        currentState = AllStates[0];
        currentState.Enter(this);
    }

    // Run the update code for the current state
	void Update ()
    {
        if (currentState != null)
        {
            // Get the state to transition to
            State nextState = currentState.StateTransitions.EvaluateTransitions();

            // Null state means no valid transition, and do not swap to the same state
            if (nextState != null && nextState.GetType() != currentState.GetType())
                SwitchState(nextState);

            currentState.Execute(this);
        }
	}

    // Swap between states
    public void SwitchState(State newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // Returns the AI team
    public Team GetTeam()
    {
        return team;
    }

    // Returns PlayerTeam
    public Team GetEnemyTeam()
    {
        return enemyTeam;
    }
}