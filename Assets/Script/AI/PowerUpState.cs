using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class is a state for gathering powerUps. I have partially implemented the functional code.
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 */

public class PowerUpState : State
{
    private Team team;
    private float radius = 20f;
    private GameObject powerUp;
    private float orbitDist = 5.0f;

    public PowerUpState(Team t)
    {
        team = t;
        Collider[] collidersInRange = Physics.OverlapSphere((team.player1.transform.position + team.player2.transform.position) / 2, radius);
        foreach(Collider obj in collidersInRange)
        {
            if (obj.gameObject.GetComponent<PowerUp>())
            {
                powerUp = obj.gameObject;
                break;
            }
        }
    }

    public override void Start(StateMachine sm)
    {
        Debug.Log("Entering State: PowerUp");
        team.player1.GetComponent<AIController>().MoveToPoint(new Vector3(powerUp.transform.position.x, 1.5f, powerUp.transform.position.z + orbitDist));
    }

    public override void Update(StateMachine sm)
    {
        Debug.Log("Current State: PowerUp");
    }

    public override void Exit(StateMachine sm)
    {
        Debug.Log("Exiting State: PowerUp");
    }

    public override State EvaluateTransitions(StateMachine sm)
    {
        Debug.Log("Evaluating Transitions...");
        return null;
    }
}
