using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: This class serves as an invincibility powerup for a team.
 * Not much needs to happen in this script, for it to work it just merely needs to exist.
 * There may be a better way to get the same functionality without needing a script.
 * 
 * Version 1.0.0
 * Author: Zachary Schmalz
 * Date: October 6, 2017
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date January 26, 2018
 * Revisions: Improved process of how Invincibility is triggered and added particle effects for the duration of the power-up
 */

public class InvincibilityPowerUp : PowerUp
{
    // Assign PowerUp data
    public override void SetData(float delay = 0, float duration = 0, float param3 = 0)
    {
        spawnDelay = delay;
        activeLength = duration;
    }

    public override void Activate()
    {
        // Add more time to the spawn timer which prevents death
        Team.spawnTimer = activeLength;

        // Add particle effects to the players
        GameObject invincibilityEffect1 = Instantiate(Team.invincibilityParticlePrefab, Player1.transform.position, Quaternion.identity, Player1.transform);
        GameObject invincibilityEffect2 = Instantiate(Team.invincibilityParticlePrefab, Player2.transform.position, Quaternion.identity, Player2.transform);
        var main1 = invincibilityEffect1.GetComponent<ParticleSystem>().main;
        var main2 = invincibilityEffect1.GetComponent<ParticleSystem>().main;
        main1.startLifetime = activeLength;
        main2.startLifetime = activeLength;
        Destroy(invincibilityEffect1, activeLength);
        Destroy(invincibilityEffect2, activeLength);

        base.Activate();
    }
}