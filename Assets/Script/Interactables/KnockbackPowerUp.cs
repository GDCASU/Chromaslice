using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPowerUp : PowerUp
{
    public float knockbackRadius;
    public float knockbackIntensity; 

	protected override void Start ()
    {
        base.Start();
	}

    // Activate the knockback
    public override void Activate()
    {
        base.Activate();

        knockbackRadius *= boostMultiplier;
        knockbackRadius *= boostMultiplier;

        // Get all colliders within the radius of player 1
        Collider[] allColliders1 = Physics.OverlapSphere(player1.transform.position, knockbackRadius);
        Collider[] allColliders2 = Physics.OverlapSphere(player2.transform.position, knockbackRadius);

        // Explosion around player1
        foreach(Collider collider in allColliders1)
        {
            // Only get players
            if(collider.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (collider.gameObject != player1.gameObject && collider.gameObject != player2.gameObject)
                {
                    Debug.Log("Team " + collider.gameObject.GetComponentInChildren<PlayerController>().team);
                    Rigidbody body = collider.GetComponent<Rigidbody>();
                    body.AddExplosionForce(knockbackIntensity, player1.transform.position, knockbackRadius);
                }
            }
        }

        // Explosion around player2
        foreach (Collider collider in allColliders2)
        {
            // Only get players
            if (collider.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (collider.gameObject != player1.gameObject && collider.gameObject != player2.gameObject)
                {
                    Debug.Log("Team " + collider.gameObject.GetComponentInChildren<PlayerController>().team);
                    Rigidbody body = collider.GetComponent<Rigidbody>();
                    body.AddExplosionForce(knockbackIntensity, player2.transform.position, knockbackRadius);
                }
            }
        }
    }
}
