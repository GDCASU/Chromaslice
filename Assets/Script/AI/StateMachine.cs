using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class serves as a Finite State Machine for the AI. The default state is Idle and
 *                  provides functionality to swap between the states. Attach this script to the AI team.
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 */

public class StateMachine : MonoBehaviour
{
    public GameObject test;

    public State currentState;
    private Team team;
    private Team enemyTeam;

    // Initial state is Idle
    void Start()
    {
        team = gameObject.GetComponent<Team>();
        enemyTeam = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Team>();

        currentState = new AttackState(test);
        currentState.Start(this);
    }

    // Run the update code for the current state
	void Update ()
    {
        if (currentState != null)
        {
            // Evaluate the transitions of the current state before running Update code. If null, no new state.
            State nextState = currentState.EvaluateTransitions(this);
            if (nextState != null)
                SwitchState(nextState);

            currentState.Update(this);
        }
	}

    // Swap between states
    public void SwitchState(State newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Start(this);
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