using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class serves as the base class for all Interactables in the game.
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 15, 2017
 * 
 * Version: 1.0.1
 * Author: Zachary Schmalz
 * Date: September 22, 2017
 * Revisions: Code optimizations
 * 
 * Version 1.1.0
 * Author: Zachary Schmalz
 * Date: September 27, 2017
 * Revisions: Added rigidBody component and Team property
*/

public class Interactable : MonoBehaviour
{
    protected Team team;
    protected GameObject player1;
    protected GameObject player2;
    protected Rigidbody rigidBody;

    public Team Team
    {
        get { return team; }
        set
        {
            team = value;
            player1 = team.player1;
            player2 = team.player2;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
            rigidBody = gameObject.GetComponent<Rigidbody>();
        else
            rigidBody = null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    // Interactables that react when a player triggers them
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Save the team and player references to be used in subclasses
        Debug.Log(other.GetComponentInParent<Rigidbody>().gameObject.name + " collided with Interactable");
        Team = other.GetComponentInParent<Team>();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {

    }
}