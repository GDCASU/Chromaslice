using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description: This class controls the soccer game mode
// Author(s): Connor Pillsbury, Zachary Schmalz
// Version: 2.0.0
// Date: March 16, 2018

public class Soccer : GameMode
{
    // Reference to the soccer ball object on the field
    public GameObject soccerBall;

	protected override void Start ()
    {
        timeRemaining = GameConstants.SoccerTimeLimit;
        base.Start();
	}
	
	protected override void Update ()
    {
        // Timer for the next round
        if (nextRoundTrigger)
        {
            if (timeBeforeNextRound > 0)
                timeBeforeNextRound -= Time.deltaTime;
            else
            {
                // Reset players
                base.KillTeam(null);

                // Stop game if round limit reached
                if (IsGameOver)
                    StopGame();

                // Reset the round and soccer ball
                else
                {
                    BeginRound();
                    soccerBall.GetComponent<SoccerBall>().Reset();
                }
            }
        }
        base.Update();
	}

    // Add score when ball collides with net collider, function called in the SoccerBall.cs script
    public override void AddScore(string name)
    {
        if (name == "Team 0")
            team1Score++;
        else if (name == "Team 1")
            team2Score++;

        NetManager.GetInstance().SendScoreUpdate();
        GameManager.singleton.WriteToLog(name + " won the round with " + timeRemaining + " seconds remaining");

        base.AddScore(name);
    }

    public override void BeginRound()
    {
        timeRemaining = GameConstants.SoccerTimeLimit;
        base.BeginRound();
    }

    // In Soccer, instead of ending the game when a player dies, respawn the player after a short timer
    // Don't know how coroutines will work will work along with networking. Sorry Kyle :/
    public override void KillTeam(GameObject player)
    {
        if(!nextRoundTrigger)
        {
            StartCoroutine(RespawnPlayer(player));
        }

        // If round ends before a score, kill all players
        else if (nextRoundTrigger && player == null)
        {
            foreach (GameObject t in GameManager.singleton.teams)
            {
                Team team = t.GetComponent<Team>();
                team.KillTeam(team.player1);
                team.KillTeam(team.player2);
            }
            timeRemaining = GameConstants.TimeBeforeNextRound;
        }
    }

    // Respawn a player after a cooldown with an invincibility period
    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(GameConstants.SoccerRespawnTime);
        player.GetComponentInParent<Team>().ResetPlayer(player);
        player.GetComponentInParent<Team>().AddInvincibilityEffect(player, GameConstants.SoccerRespawnInvincibility);
    }
}