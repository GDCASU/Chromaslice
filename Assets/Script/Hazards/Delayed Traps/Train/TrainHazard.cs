using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by:          Tyler Cole
// Date:                2/1/2018
// Description:         The TrainHazard class is the class that will be used by all trains (prefab should be included).
//                      The train stays active for the time put in DurationActive, then despawns, then respawns in its
//                      original position (startLocation) after it has gone through the time defined by DurationInactive. 
//                      A basic movement function has also been added, but is open to be removed when a movement script
//                      is added to the train.  

public class TrainHazard : MonoBehaviour // not complete
{

    float spawnedTime;
    float activeTime;
    public float durationInactive;  // the float used to determine the time (in seconds) of how long the train is inactive
    public float durationActive;    // the float used to determine the time (in seconds) of how long the train is active
    Vector3 startLocation;
    public GameObject train;        // put the train object here
    bool isTrainActive = true;
    public float trainSpeed;        // the float used to determine train's speed (delete if a movement script is created)

    // Use this for initialization
    void Start()
    {
        spawnedTime = Time.time;

        // start location is determined by placement
        startLocation = train.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrainActive)
        {
            // very basic movement (delete if a movement script is created)
            train.transform.Translate(Vector3.forward * Time.deltaTime * trainSpeed);

            // despawns train after durationActive time
            if (Time.time - durationActive >= spawnedTime)
            {
                activeTime = Time.time;
                DespawnTrain();
                isTrainActive = false;
            }
        }
        else
        {
            // respawns train after durationInactive
            if (Time.time - durationInactive >= activeTime)
            {
                spawnedTime = Time.time;
                RespawnTrain();
                isTrainActive = true;
            }
        }
    }

    // if it hits the player it should knockback and debuff (still in development, currently just kills)
    void OnTriggerEnter(Collider c)
    {
        // Knockback
        if (isTrainActive)
        {
            // knockback ?
            // c.GetComponent<Rigidbody>().AddForce(c.relativeVelocity(.normal, ForceMode.VelocityChange);
            // debuff
        }
    }

    // sets the train back to active and then resets it to starting location
    void RespawnTrain()
    {
        train.SetActive(true);
        train.transform.position = startLocation;
    }
    
    // sets the train to deactive
    void DespawnTrain()
    {
        train.SetActive(false);
    }
}
