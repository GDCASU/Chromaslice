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

// Developer:   Tyler Cole
// Date:        2/19/2018
// Description: Turned into a child class of hazard and updated to fit that. *IMPORTANT* A collision script needs to be
//              written for the train gameObject.

public class TrainHazard : Hazard 
{

    Vector3 startLocation;
    public GameObject train;        // put the train object here
    public float trainSpeed;        // the float used to determine train's speed (delete if a movement script is created)

    // Initalize timer
    public override void Start()
    {
        base.Start();
        startLocation = train.transform.position;
    }

    // Handles timer and toggling hazard status
    public override void Update()
    {
        base.Update();
        train.SetActive(isActive);
        if (isActive)
        {
            train.transform.Translate(Vector3.forward * Time.deltaTime * trainSpeed);
        }
    }

    // Resets the timer of the trap
    public override void ResetTimer()
    {
        base.ResetTimer();
    }

    // Activates the trap
    public override void ActivateTrap()
    {
        base.ActivateTrap();
        train.transform.position = startLocation;
    }

    // Deactivates the trap
    public override void DeactivateTrap()
    {
        base.DeactivateTrap();
    }

    // Behavior for when another object collides with this object
    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    // Behavior for when another object stops colliding with this object
    public override void OnCollisionExit(Collision other)
    {
        base.OnCollisionExit(other);
    }

    // Behavior for when another object is touching this object
    public override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
    }

    // Behavior for when another objects trigger touches this object
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    // Behavior for when another objects trigger stops touching this object
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    // Behavior for when another objects trigger is touching this object
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }
}
