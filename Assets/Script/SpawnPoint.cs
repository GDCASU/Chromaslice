using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * SpawnPoint is a parent of spawnTeams and gets the positions of the children
 * to send them to GameManager
 * 
 * Date created: 9/16/17 Connor Pillsbury
 * Revised: 9/17/17 Connor Pillsbury
 */

//revised 9/21/17 by Kyle Aycock: modified to use new spawn system

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Networking update

public class SpawnPoint : NetworkBehaviour
{
    public GameManager game;

    public override void OnStartServer()
    {
        game = GameManager.singleton;
        game.SetNumberOfPlayers(transform.childCount); //Tells GameManager number of teams to make
        foreach (Transform t in transform)
            RegisterTeamSpawn(t.GetSiblingIndex(), t.GetChild(0).position, t.GetChild(1).position);
    }

    [Server]
    public void RegisterTeamSpawn(int index, Vector3 first, Vector3 second)
    {
        game.SetSpawn(index, first, second);
    }
}
