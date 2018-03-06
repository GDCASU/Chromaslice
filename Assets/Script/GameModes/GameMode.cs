using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMode : NetworkBehaviour
{
    [SyncVar] protected float team1Score;
    [SyncVar] protected float team2Score;

    protected int gameRoundLimit;
    protected int currentRound;
    protected float timeLimit;
    protected bool gameActive;
    protected float timeRemaining;
    protected float timeBeforeRound;

    public bool IsGameActive { get { return gameActive; } }
    public bool IsRoundOver { get { return timeRemaining <= 0;} }
    public float TimeReamining { get { return timeRemaining; } protected set { timeRemaining = value; } }
    public int CurrentRound { get { return currentRound; } protected set { currentRound = value; } }
    public int GameRoundLimit { get { return gameRoundLimit; } set { gameRoundLimit = value; } }
    public float Team1Score { get { return team1Score; } }
    public float Team2Score { get { return team2Score; } }


    protected virtual void Start()
    {
        team1Score = 0;
        team2Score = 0;
        currentRound = 0;
        gameActive = false;
        timeRemaining = GameConstants.DeathmatchTimeLimit;
        timeBeforeRound = GameConstants.TimeBeforeRound;
    }

    protected virtual void Update ()
    {
        if(gameActive)
        {
            if (timeBeforeRound > 0)
                timeBeforeRound -= Time.deltaTime;

            else if (!IsRoundOver)
                timeRemaining -= Time.deltaTime;

            else if(IsRoundOver)
            {
                KillTeam(null);
                GameManager.singleton.WriteToLog("Time Limit Reached! Draw!");
            }
        }
	}

    // Base behavior: Reset team to spawn
    [Server]
    public virtual void KillTeam(Team team)
    {
        for (int i = 0; i < GameManager.singleton.teams.Length; i++)
        {
            Team t = GameManager.singleton.teams[i].GetComponent<Team>();
            t.ResetTeam();
            t.RpcResetTeam();
        }
    }

    public void BeginRound()
    {
        timeBeforeRound = GameConstants.TimeBeforeRound;
        timeRemaining = GameConstants.DeathmatchTimeLimit;
        gameActive = true;
    }

    public virtual void AddScore(string name)
    {

    }

    public void GameWinner()
    {
        if (team1Score > team2Score)
            GameManager.singleton.WriteToLog("Round End: Team 1 wins!");
        else if (team2Score > team1Score)
            GameManager.singleton.WriteToLog("Round End: Team 2 wins!");
        else
            GameManager.singleton.WriteToLog("Round End: Both teams tied!");
    }
}
