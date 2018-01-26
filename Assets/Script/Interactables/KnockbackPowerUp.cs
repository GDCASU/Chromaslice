using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:  This power-up emits an explosive force around the players to knockback other players
 * Version 1.0.0
 * Author: Zachary Schmalz
 * Date: November 1, 2018
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 * Revisions: Added funcionality to work with power-up spawners
 */

public class KnockbackPowerUp : PowerUp
{
    private float knockbackRadius;
    private float knockbackIntensity;

    // Assign power-up data
    public override void SetData(float delay = 0, float radius = 0, float intensity = 0)
    {
        spawnDelay = delay;
        knockbackRadius = radius;
        knockbackIntensity = intensity;
    }

    // Activate the knockback
    public override void Activate()
    {
        base.Activate();

        // Get all colliders within the radius of player 1
        Collider[] allColliders1 = Physics.OverlapSphere(Player1.transform.position, knockbackRadius);
        Collider[] allColliders2 = Physics.OverlapSphere(Player2.transform.position, knockbackRadius);

        // Explosion around Player1
        foreach(Collider collider in allColliders1)
        {
            // Only get players
            if(collider.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (collider.gameObject != Player1.gameObject && collider.gameObject != Player2.gameObject)
                {
                    Debug.Log("Team " + collider.gameObject.GetComponentInChildren<PlayerController>().team);
                    Rigidbody body = collider.GetComponent<Rigidbody>();
                    body.AddExplosionForce(knockbackIntensity, Player1.transform.position, knockbackRadius);
                }
            }
        }

        // Explosion around Player2
        foreach (Collider collider in allColliders2)
        {
            // Only get players
            if (collider.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (collider.gameObject != Player1.gameObject && collider.gameObject != Player2.gameObject)
                {
                    Debug.Log("Team " + collider.gameObject.GetComponentInChildren<PlayerController>().team);
                    Rigidbody body = collider.GetComponent<Rigidbody>();
                    body.AddExplosionForce(knockbackIntensity, Player2.transform.position, knockbackRadius);
                }
            }
        }
    }
}
