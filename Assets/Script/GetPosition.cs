using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/*
* GetPosition gets the position of the current object and sends it to SpawnPoint
* to set the spawns of the teams
* 
* Date created: 9/17/17 Connor Pillsbury
*/

//revised 9/21/17 by Kyle Aycock: modified to work with new spawn system

public class GetPosition : NetworkBehaviour
{
    public SpawnPoint spawn;

    void Start()
    {
        if(isServer)
            spawn.RegisterTeamSpawn(transform.GetSiblingIndex(), transform.GetChild(0).position, transform.GetChild(1).position);
    }
}
