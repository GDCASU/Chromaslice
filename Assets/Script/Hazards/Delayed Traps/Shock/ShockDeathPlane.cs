using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Paul Gellai - 10/31/17 - custom death plane script for Shock trap

public class ShockDeathPlane : MonoBehaviour {

    float killStartTime;
    float lifeTime;
    public float durationUntilKillable;
    public float lifetimeAfterKillable;
    bool canKill = false;
    bool lifeTimeStarted = false;

	// Use this for initialization
	void Start () {
        killStartTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {

        // this script makes the plane kill players after a certain duration, and then it is destroyed after a certain duration 
        // the color of the shock death plane changes to a screwed up color once it can kill (this is very temporary)

		if(Time.time - durationUntilKillable >= killStartTime)
        {
            canKill = true;
        }
        if(canKill)
        {

            if(!lifeTimeStarted)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = new Color(255f, 0f, 255f);
                lifeTime = Time.time;
                lifeTimeStarted = true;
            }
            if(Time.time - lifetimeAfterKillable >= lifeTime)
            {
                DestroyObject(gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if(canKill)
        {
            c.transform.parent.GetComponent<Team>().KillTeam();
        }
    }
}
