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
*/

public abstract class Interactable : MonoBehaviour
{
    protected Team team;
    protected GameObject player1;
    protected GameObject player2;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    // Interactables that react when a player triggers them
    // Currently only PowerUp's are activated via triggers
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Save the team and player references to be used in subclasses
        Debug.Log(other.GetComponentInParent<Rigidbody>().gameObject.name + " collided with Interactable");
        team = other.GetComponentInParent<Team>();
        player1 = team.player1;
        player2 = team.player2;
        // Once a player triggers an Interactable, hide the Interactable from the scene and prevent other GameObjects from triggering it
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}