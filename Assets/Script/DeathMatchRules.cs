using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            9/15/2017
// Description:     Rules to determine if game is over for Death Match gamemode

public class DeathMatchRules : MonoBehaviour, GameRules
{
    public int scoreLimit { get; set; }
    public float timeLimit { get; set; }

    // Use this for initialization
    void Start()
    {
        scoreLimit = 5;
        timeLimit = 60f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Checks if game is over given the games time limit and scores of the teams
    public bool IsGameOver(int scoreTeam1, int scoreTeam2, float time)
    {
        bool gameOver = false;

        if (scoreTeam1 >= scoreLimit || scoreTeam2 >= scoreLimit)
            gameOver = true;

        if (time >= timeLimit)
            gameOver = true;

        return gameOver;
    }
}
