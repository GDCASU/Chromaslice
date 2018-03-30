using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Description: This class controls all functions of an active game and serves as the base class for all gamemodes
// Author(s): Zachary Schmalz, (others to be credited)
// Version 2.0.0
// Date: March 16, 2018

public class GameMode : MonoBehaviour
{
    // Protected fields to be used im base/sub classes
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

    // Private variables
    private GameObject camera;
    private float animationTimer;

    // Public properties
    public bool IsGameActive { get { return gameActive; } }
    public bool IsCountdownActive { get { return timeBeforeRound < GameConstants.TimeBeforeRound && timeBeforeRound > 0; } }
    public bool IsRoundActive { get { return gameActive && timeBeforeRound <= 0 && timeRemaining > 0; } }
    public bool IsRoundOver { get { return timeRemaining <= 0;} }
    public float TimeReamining { get { return timeRemaining; } protected set { timeRemaining = value; } }
    public int CurrentRound { get { return currentRound; } protected set { currentRound = value; } }
    public int GameRoundLimit { get { return gameRoundLimit; } set { gameRoundLimit = value; } }
    public float Team1Score { get { return team1Score; } }
    public float Team2Score { get { return team2Score; } }
    public bool IsGameOver { get { return currentRound >= gameRoundLimit; } }

    // Initiailize variables
    protected virtual void Start()
    {
        team1Score = 0;
        team2Score = 0;
        currentRound = 0;
        gameActive = false;
        nextRoundTrigger = false;
        timeBeforeRound = GameConstants.TimeBeforeRound;
        animationTimer = 0;
        camera = null;
    }

    protected virtual void Update ()
    {
        // Only begin updates when scene has changhed to the level
        if (SceneManager.GetActiveScene().name != GameManager.singleton.level)
            return;

        // If the level has a camera flyby animation
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
            if (camera.GetComponent<Animator>())
                animationTimer = camera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        }

        // The camera is animating
        if (animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer < 0)
                animationTimer = 0;
        }

        // The scene is actively open and executing
        else if (gameActive)
        {
            // Execute only once at the beginning of the countdown timer
            if (timeBeforeRound == GameConstants.TimeBeforeRound)
            {
                // Reset/Trigger players + animations
                for (int i = 0; i < GameManager.singleton.teams.Length; i++)
                    GameManager.singleton.teams[i].GetComponent<Team>().RpcResetTeam();
            }

            // Wait for time before the round starts
            if (timeBeforeRound > 0)
            {
                timeBeforeRound -= Time.deltaTime;
            }

            // Subtract time from the remaining time in the round
            else if (!IsRoundOver)
                timeRemaining -= Time.deltaTime;

            // The time expired, reset the round
            else if (IsRoundOver && !nextRoundTrigger)
            {
                nextRoundTrigger = true;
                KillTeam(null);
                GameManager.singleton.WriteToLog("Time Limit Reached! Draw!");
            }
        }
    }

    // Base behavior: Reset team to spawn
    public virtual void KillTeam(GameObject player)
    {
        for (int i = 0; i < GameManager.singleton.teams.Length; i++)
        {
            Team t = GameManager.singleton.teams[i].GetComponent<Team>();
            t.RpcResetTeam();
        }
    }

    // Resets all variables used/changed during a round
    public virtual void BeginRound()
    {
        timeBeforeRound = GameConstants.TimeBeforeRound;
        timeBeforeNextRound = GameConstants.TimeBeforeNextRound;
        nextRoundTrigger = false;
        gameActive = true;
    }

    // Sub classes handle updating team scores, the base behavior handles transitioning to the next round (or game end)
    public virtual void AddScore(string name)
    {
        nextRoundTrigger = true;
        currentRound++;
        if (IsGameOver)
        {
            timeBeforeNextRound = GameConstants.TimeBeforeGameEnd;
            timeRemaining = timeBeforeNextRound;
        }
        else
            timeRemaining = GameConstants.TimeBeforeNextRound;
    }

    // Display the results of the match
    public void GameWinner()
    {
        if (team1Score > team2Score)
            GameManager.singleton.WriteToLog("Round End: Team 1 wins!");
        else if (team2Score > team1Score)
            GameManager.singleton.WriteToLog("Round End: Team 2 wins!");
        else
            GameManager.singleton.WriteToLog("Round End: Both teams tied!");
    }

    // Handles the stopping of the game and transitioning to title screen
    public virtual void StopGame()
    {
        GameWinner();

        gameActive = false;
        GameManager.singleton.activePlayers = 0;

        // Set the offline scene to "Title" then stop the server and switch to it
        NetManager.GetInstance().offlineScene = "Title"; // change to server you want to change to
        if (GameManager.singleton.isLocalPlayer)
        {
            NetManager.GetInstance().StopClient();
        }
        else
        {
            NetManager.GetInstance().StopServer();
        }
    }
}