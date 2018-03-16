using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Description: This class controls the deathmath game mode
// Author(s): Connor Pillsbury, Zachary Schmalz, (others to be credited)
// Version: 2.0.0
// Date: March 16, 2018

public class Deathmatch : GameMode
{
	protected override void Start ()
    {
        timeRemaining = GameConstants.DeathmatchTimeLimit;
        base.Start();
	}
	
	protected override void Update ()
    {
        // Timer to the next round (or game end) has been triggered
        if (nextRoundTrigger)
        {
            if (timeBeforeNextRound > 0)
                timeBeforeNextRound -= Time.deltaTime;
            else
            {
                // Reset players
                base.KillTeam(null);

                if (IsGameOver)
                    StopGame();
                else
                    BeginRound();
            }
        }

        base.Update();
    }

    public override void BeginRound()
    {
        timeRemaining = GameConstants.DeathmatchTimeLimit;
        base.BeginRound();
    }

    // When a player dies, update the score for the appropriate team
    public override void KillTeam(GameObject player)
    {
        if (!nextRoundTrigger)
        {
            Team team = player.GetComponentInParent<Team>();
            for (int i = 0; i < GameManager.singleton.teams.Length; i++)
            {
                Team t = GameManager.singleton.teams[i].GetComponent<Team>();
                if (t != team && team != null)
                {
                    AddScore(t.name);
                    NetManager.GetInstance().SendScoreUpdate();
                    GameManager.singleton.WriteToLog(t.name + " won the round with " + timeRemaining + " seconds remaining");
                }
            }
        }

        // Time expired without a death, destroy players and reset round
        else if(nextRoundTrigger && player == null)
        {
            foreach(GameObject t in GameManager.singleton.teams)
            {
                Team team = t.GetComponent<Team>();
                team.KillTeam(team.player1);
                team.KillTeam(team.player2);
            }
            timeRemaining = GameConstants.TimeBeforeNextRound;
        } 
    }

    // Add score and reset round
    public override void AddScore(string name)
    {
        if (name == "Team 0")
            team1Score++;
        else if(name == "Team 1")
            team2Score++;

        base.AddScore(name);
    }
}