using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Nick Arnieri
// Date:        1/26/2017
// Description: Template class for hazards

// Developer:   Nick Arnieri
// Date:        2/22/2017
// Description: Add collision events

public class PotHazard : Hazard
{
	public float fallHeight;
	Vector3 startLocation;
	public GameObject myTrigger;
	/// <summary>
	/// Initalize timer
	/// </summary>
	public override void Start()
	{
		base.Start();
		startLocation = transform.position;
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Handles timer and toggling hazard status
	/// </summary>
	public override void Update()
	{
		Debug.Log (myTrigger.GetComponent<PotTrigger> ().isInRange());
		if (myTrigger.GetComponent<PotTrigger> ().isInRange() || isActive) {
			base.Update ();
		}
		// ADD YOUR CODE HERE
		if (isActive) {
			transform.position = new Vector3(startLocation.x, startLocation.y + (fallHeight)*(timer)/activeLength, startLocation.z);
		} else {
			transform.position = new Vector3(startLocation.x, startLocation.y, startLocation.z);
		}
	}

	/// <summary>
	/// Resets the timer of the trap
	/// </summary>
	public override void ResetTimer()
	{
		base.ResetTimer();
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Activates the trap
	/// </summary>
	public override void ActivateTrap()
	{
		base.ActivateTrap();
		// ADD YOUR CODE HERE
		transform.position = new Vector3(startLocation.x, startLocation.y + (fallHeight), startLocation.z);
	}

	/// <summary>
	/// Deactivates the trap
	/// </summary>
	public override void DeactivateTrap()
	{
		base.DeactivateTrap();
		transform.position = new Vector3(startLocation.x, startLocation.y + (fallHeight), startLocation.z);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another object collides with this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another object stops colliding with this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnCollisionExit(Collision other)
	{
		base.OnCollisionExit(other);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another object is touching this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnCollisionStay(Collision other)
	{
		base.OnCollisionStay(other);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another objects trigger touches this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another objects trigger stops touching this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
		// ADD YOUR CODE HERE
	}

	/// <summary>
	/// Behavior for when another objects trigger is touching this object
	/// </summary>
	/// <param name="other">Game object colliding with</param>
	public override void OnTriggerStay(Collider other)
	{
		base.OnTriggerStay(other);
		// ADD YOUR CODE HERE
	}
}
