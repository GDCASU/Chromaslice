using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class inherits from Interactable. This script would be attached to items in the scene
 * that teams can slingshot using their rope. The physics and the rope will require some more fine-tuning
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 27, 2017
 */

public class Projectile : Interactable
{
    private Rigidbody rigidBody;

	// Use this for initialization
	protected override void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        
	}
    
    // Function to calculate direction and velocity to launch projectile
    public virtual void LaunchProjectile(List<Vector3> ropePoints)
    {
        // Calculate the average direction of all points on the rope
        Vector3 averageDirection = new Vector3();
        for(int i = 0; i < ropePoints.Count - 1; i++)
            averageDirection += ropePoints[i];

        averageDirection /= ropePoints.Count - 1;

        Vector3 speedModifier = (Player1.GetComponent<Rigidbody>().velocity + Player2.GetComponent<Rigidbody>().velocity) / 2;
        Vector3 direction = gameObject.transform.position - averageDirection;

        // These values may need adjusting
        direction.x *= Mathf.Abs(speedModifier.x) * 5;
        direction.y = 7; 
        direction.z *= Mathf.Abs(speedModifier.z) * 5;

        rigidBody.velocity = direction;
    }
}
