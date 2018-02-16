using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            9/15/2017
// Description:     Rules to determine if game is over for Death Match gamemode

// Developer:       Nick Arnieri
// Date:            10/4/2017
// Description:     Change to make game rules responsible for keeping track of score

// Developer:       Nick Arnieri
// Date:            10/20/2017
// Description:     Seperate function calls to allow easier usage by GameManager

public class Deathmatch : GameMode
{
    public int scoreLimit;

    private bool gameActive;

    // Use this for initialization
    void Start()
    {
        scoreTeam1 = 0;
        scoreTeam2 = 0;
        scoreLimit = 5;
        timeLimit = 60f;
        time = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
            time -= Time.deltaTime;
    }

    /// <summary>
    /// Adds a point to the necessary team
    /// </summary>
    /// <param name="name">Name of team that needs a point</param>
    public void AddScore(string name)
    {
        if (name == "Team 0")
            scoreTeam1++;
        else
            scoreTeam2++;

        gameActive = false;
    }

    /// <summary>
    /// Resets values for next match
    /// </summary>
    public void Reset()
    {
        time = timeLimit;
        gameActive = true;
    }

    /// <summary>
    /// Checks to see if game has ended based on game rules
    /// </summary>
    public string GameWinner()
    {
        string gameOver = "";

        // Check both teams score against score limit
        if (scoreTeam1 >= scoreLimit)
            gameOver = "Team 0";
        else if (scoreTeam2 >= scoreLimit)
            gameOver = "Team 1";

        return gameOver;
    }

    /// <summary>
    /// Checks to see whether the match time has run out
    /// </summary>
    public bool TimeLimit()
    {
        if (time >= 0)
            return true;

        return false;
    }
}
