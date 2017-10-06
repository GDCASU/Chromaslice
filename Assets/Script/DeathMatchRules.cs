using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            9/15/2017
// Description:     Rules to determine if game is over for Death Match gamemode

// Developer:       Nick Arnieri
// Date:            10/4/2017
// Description:     Change to make game rules responsible for keeping track of score

public class DeathMatchRules : MonoBehaviour
{
    public int scoreLimit;
    public float timeLimit;
    private int scoreTeam1;
    private int scoreTeam2;
    private float time;
    private bool gameActive;

    // Use this for initialization
    void Start()
    {
        scoreLimit = 5;
        timeLimit = 60f;
        time = timeLimit;
        gameActive = true;
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
    /// <param name="team">Team that was killed</param>
    /// <param name="teams">Array of all current teams</param>
    public void AddScore(Team team, GameObject[] teams)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            Team t = teams[i].GetComponent<Team>();
            if (t != team && team != null)
            {
                if (t.name == "Team 1")
                    scoreTeam1++;
                else
                    scoreTeam2++;
            }
        }
        // Reset timer
        time = timeLimit;
    }

    /// <summary>
    /// Checks to see if game has ended based on game rules
    /// </summary>
    public bool IsGameOver()
    {
        bool gameOver = false;

        // Check both teams score against score limit
        if (scoreTeam1 >= scoreLimit || scoreTeam2 >= scoreLimit)
            gameOver = true;

        // Check if game time has ran out
        if (time <= 0)
            gameOver = true;

        return gameOver;
    }
}
