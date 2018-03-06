using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Deathmatch : GameMode
{

	protected override void Start ()
    {
        base.Start();
	}
	
	protected override void Update ()
    {
        base.Update();
	}

    [Server]
    public override void KillTeam(Team team)
    {
        for (int i = 0; i < GameManager.singleton.teams.Length; i++)
        {
            Team t = GameManager.singleton.teams[i].GetComponent<Team>();
            t.ResetTeam();
            t.RpcResetTeam();
            if (t != team && team != null)
            {
                t.AddPoints();
                AddScore(t.name);
                NetManager.GetInstance().SendScoreUpdate();
                GameManager.singleton.WriteToLog(t.name + " won the round with " + timeRemaining + " seconds remaining");
            }
            currentRound++;
            timeBeforeRound = GameConstants.TimeBeforeRound;
            if(currentRound >= gameRoundLimit)
            {
                gameActive = false;
                NetManager.GetInstance().StopHost();
                GameManager.singleton.activePlayers = 0;
            }

        }
    }

    public override void AddScore(string name)
    {
        if (name == "Team 0")
            team1Score++;
        else
            team2Score++;

        gameActive = false;
    }
}