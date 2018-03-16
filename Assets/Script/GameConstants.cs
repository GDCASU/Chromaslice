using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Description: This class contains widely used and common constants to be referenced anywhere in code. Allows for easy updating of values
// Author: Zachary Schmalz
// Version: 1.0.0
// Date: March 16, 2018

public static class GameConstants
{
    // GameMode Constants
    public const float TimeBeforeRound = 3.0f;                  // Time before each round begins
    public const float TimeBeforeNextRound = 5.0f;              // Time before starting the next round
    public const float TimeBeforeGameEnd = 7.0f;                // Time before ending the games and transitioning to title screen

    // Deathmatch Constants
    public const float DeathmatchTimeLimit = 60.0f;             // Time limit for each round in Deathmatch

    // Soccer Constants
    public const float SoccerTimeLimit = 60.0f;                 // Time limit for each round in Soccer
    public const float SoccerRespawnTime = 2.0f;                // Time before killed players respawn in Soccer
    public const float SoccerRespawnInvincibility = 2.0f;       // Time that respawned players have spawn invincibility in Soccer
}