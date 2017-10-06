using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SpawnPoint is a parent of spawnTeams and gets the positions of the children
 * to send them to GameManager
 * 
 * Date created: 9/16/17 Connor Pillsbury
 * Revised: 9/17/17 Connor Pillsbury
 */

//revised 9/21/17 by Kyle Aycock: modified to use new spawn system

public class SpawnPoint : MonoBehaviour
{
    public GameManager game;

    void Start()
    {
        game = GameManager.singleton;
        game.SetNumberOfPlayers(transform.childCount); //Tells GameManager number of teams to make
    }

    public void RegisterTeamSpawn(int index, Vector3 first, Vector3 second)
    {
        game.SetSpawn(index, first, second);
    }
}
