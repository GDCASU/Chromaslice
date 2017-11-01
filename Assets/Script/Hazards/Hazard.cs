using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ava Warfield 9/20/17

public class Hazard : MonoBehaviour
{
    public virtual void PlayerInteract()
    {
        // insert code here to control behaviour upon interaction with player objects

        // override in children using "public override void PlayerInteract()"
    }


    // when player 
    void OnTriggerEnter(Collider other)
    {
        other.transform.parent.GetComponent<Team>().KillTeam();
    }
}
