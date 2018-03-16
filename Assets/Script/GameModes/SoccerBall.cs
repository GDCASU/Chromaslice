using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description: This class is attached to a soccer ball and checks for collisions with each team's goal
// Author: Zachary Schmalz
// Version: 1.0.0
// Date: March 16, 2018

public class SoccerBall : MonoBehaviour
{
    public GameObject RedTeamNet;
    public GameObject BlueTeamNet;
    public GameObject scoreParticlePrefab;

    private Vector3 spawnPosition;
    private bool hasScored;

	void Start ()
    {
        spawnPosition = transform.position;
        hasScored = false;

        GameManager.singleton.currentGame.GetComponent<Soccer>().soccerBall = gameObject;
	}
	
	void Update ()
    {
		
	}

    // Ball collided with net
    public void OnTriggerEnter(Collider other)
    {
        // Add score to the appropriate team and instantiate particle effect
        if (hasScored == false)
        {
            if (other.gameObject == RedTeamNet)
                GameManager.singleton.currentGame.AddScore("Team 1");

            else if (other.gameObject == BlueTeamNet)
                GameManager.singleton.currentGame.AddScore("Team 0");

            GameObject instance = Instantiate(scoreParticlePrefab, transform.position, Quaternion.identity);
            Destroy(instance, 5.0f);

            hasScored = true;
        }
    }

    // Reset the ball properties
    public void Reset()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = spawnPosition;
        hasScored = false;
    }
}