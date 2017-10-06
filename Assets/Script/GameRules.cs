using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            9/15/2017
// Description:     Generic interface for various gamemodes to setup game over conditions

public interface GameRules
{
    int scoreLimit { get; set; }
    float timeLimit { get; set; }
    bool IsGameOver(int scoreTeam1, int scoreTeam2, float time);
}
