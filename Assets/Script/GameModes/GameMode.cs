using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMode : MonoBehaviour
{
    protected float team1Score;
    protected float team2Score;

    protected int gameRoundLimit;
    protected int currentRound;
    protected float timeLimit;
    protected bool gameActive;
    protected float timeRemaining;
    protected float timeBeforeRound;
    protected float timeBeforeNextRound;
    protected bool nextRoundTrigger;

    private GameObject camera;
    private float animationTimer;

    public bool IsGameActive { get { return gameActive; } }
    public bool IsRoundActive { get { return gameActive && timeBeforeRound <= 0 && timeRemaining > 0; } }
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
        nextRoundTrigger = false;
        timeRemaining = GameConstants.DeathmatchTimeLimit;
        timeBeforeRound = GameConstants.TimeBeforeRound;
        animationTimer = 0;
        camera = null;
    }

    protected virtual void Update ()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
            if (camera.GetComponent<Animator>())
                animationTimer = camera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        }

        if (animationTimer > 0)
                animationTimer -= Time.deltaTime;

        else if (gameActive)
        {
            if (timeBeforeRound > 0)
                timeBeforeRound -= Time.deltaTime;

            else if (!IsRoundOver)
                timeRemaining -= Time.deltaTime;

            else if(IsRoundOver && !nextRoundTrigger)
            {
                KillTeam(null);
                GameManager.singleton.WriteToLog("Time Limit Reached! Draw!");
            }
        }
    }

    // Base behavior: Reset team to spawn
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
        timeBeforeNextRound = GameConstants.TimeBeforeNextRound;
        nextRoundTrigger = false;
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
