using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saturday, April 14
// Kyle Chapman
// Created the class

public class PushUpFan : MonoBehaviour
{
	public float fanForce = 100f;
	public float forceDepth = 5f;

	void OnTriggerStay (Collider other)
	{
		if (other.tag.Equals("Player") || other.tag.Equals("AIPlayer"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();

			if (rb)
			{
                SoundManager.singleton.PlaySoundInstance("FanEffect");
				rb.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, fanForce, 0), this.transform.position - Vector3.down * forceDepth, ForceMode.Force);
			}
		}
	}
}
