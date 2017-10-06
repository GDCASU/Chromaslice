using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * GameManager controls spawning the players in the beginning of the game, 
 * it controls the amount of players allowed in the game.
 * 
 * Date created: 9/15/17 Connor Pillsbury
 * Revised: 9/20/17 Connor Pillsbury
 */

// Developer:   Kyle Aycock
// Date:        9/21/17
// Description: Fixed documentation (changed to C# XML)
//              Changed spawn point system to allow for individual spawn points for each player of a team
//              Replaced IsDestroyed with KillTeam, which resets team positions and advances round counter by 1
//              Added round counter, returns to title screen after a match (set of rounds) completes

// Developer:   Paul Gellai
// Date:        9/27/17
// Description: Added matchStarted boolean to make sure that the game timer does not begin counting down until 
// the countdown timer has finished. Added countdown timer for 3..2..1 at beginning of rounds.

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public int firstTeamLayer;
    public bool singleControllerPerTeam;
    public GameObject teamPrefab;
    public GameObject[] teams;
    public float matchTime;
    public Vector3[] spawnPoints;
    public int numberOfPlayers;
    public DeathMatchRules deathMatch;
    public KingOfTheHillRules hillRules;
    public int maxRounds;
    public int currentRound;
    public bool gameActive;
    public bool matchStarted = false;
    public bool useTitleScreen;
    public bool countdownOver = false;
    public float timer;
    public float countdownTimer;
    public float timeBeforeMatch;
    public float spawnTimer;

    public Color[,] colorPairs = { { new Color(255, 0, 0), new Color(255, 50, 0) }, { new Color(0, 0, 255), new Color(0, 150, 255) } }; //red, orange, blue, cyan

    private string outputPath;

    // Use this for initialization
    void Awake()
    {
        if (singleton)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this);
        outputPath = Application.dataPath + "/gamelog.txt";
        if (!File.Exists(outputPath)) File.Create(outputPath).Close();
        Debug.Log("Logging match results to: " + outputPath);
        gameActive = false;
        countdownTimer = timeBeforeMatch;
        timer = matchTime;
        if(!useTitleScreen)
            StartGame(SceneManager.GetActiveScene().name, maxRounds);
    }

    // Update is called once per frame
    void Update()
    {
        //Game.IsGameOver(scoreTeam1, scoreTeam2, time - from game start);

        //Simple temp timer. Need to change, but good for Friday
        if (gameActive && matchStarted)
        {
            countdownTimer -= Time.deltaTime;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                KillTeam(null); //draw
                writeToLog("Time ran out, it's a draw");
                timer = matchTime;
            }
            // if the game is active but match has not started (begins countdown timer)
        } else if(gameActive)
        {
            countdownTimer -= Time.deltaTime;
            if(countdownTimer < 1)
            {
                matchStarted = true;
            }
        }
        if(countdownTimer < 0)
            countdownOver = true;
            
    }


    /// <summary>
    /// Performs necessary actions when one team is defeated
    /// Adds points, advances current round, returns to title if rounds are over
    /// </summary>
    /// <param name="team">Team that got killed</param>
    public void KillTeam(Team team)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            Team t = teams[i].GetComponent<Team>();
            t.ResetTeam();
            if (t != team && team != null)
            {
                t.AddPoints(); //only reasonable for 2-team situations (which is probably all we're gonna have)
                writeToLog(t.name + " won the round with " + timer + " seconds remaining");
            }
        }
        currentRound++;
        timer = matchTime;
        // added 3 new variables here:
        countdownTimer = timeBeforeMatch;
        // old boolean variables - Paul
        if (currentRound >= maxRounds)
        {
            if (teams[0].GetComponent<Team>().points > teams[1].GetComponent<Team>().points)
                writeToLog("Match over. Winner: " + teams[0].name);
            else
                writeToLog("Match over. Winner: " + teams[1].name);
            gameActive = false;
            if (useTitleScreen)
                SceneManager.LoadScene(0); //return to title screen
            else
                StartGame(SceneManager.GetActiveScene().name, maxRounds);
        }
    }

    /// <summary>
    /// Sets the spawn points for the given team
    /// Calls SpawnTeam to spawn said team
    /// </summary>
    /// <param name="team">Team number</param>
    /// <param name="spawn1">Spawn point for player 1</param>
    /// <param name="spawn2">Spawn point for player 2</param>
    public void SetSpawn(int team, Vector3 spawn1, Vector3 spawn2)
    {
        spawnPoints[team * 2] = spawn1;
        spawnPoints[team * 2 + 1] = spawn2;
        SpawnTeam(team);
    }

    /// <summary>
    /// Spawns the given team object, passing it necessary info
    /// </summary>
    /// <param name="num">Which team to spawn/param>
    public void SpawnTeam(int num)
    {
        Debug.Log("Spawning team " + num);
        teams[num] = Instantiate(teamPrefab);
        teams[num].name = "Team " + num;
        teams[num].layer = firstTeamLayer + num;
        teams[num].GetComponent<Team>().SetSpawnPoints(spawnPoints[num * 2], spawnPoints[num * 2 + 1]);
        teams[num].GetComponent<Team>().SetControls(""+(num*2+1),(singleControllerPerTeam ? (num * 2 + 1)+".5" : (num * 2 + 2)+""));
        teams[num].GetComponent<Team>().SetColors(colorPairs[num, 0], colorPairs[num, 1]);
        teams[num].GetComponent<Team>().SpawnPlayer(1);
        teams[num].GetComponent<Team>().SpawnPlayer(2);
    }

    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
        teams = new GameObject[num];
        spawnPoints = new Vector3[num * 2];
    }

    public void StartGame(string levelName, int rounds)
    {
        maxRounds = rounds;
        currentRound = 0;
        gameActive = true;
        // New boolean variable position
        matchStarted = false;
        countdownOver = false;
        SceneManager.LoadScene(levelName);
        writeToLog("Starting new game, level: " + levelName + " out of " + rounds + " rounds");
    }

    public void writeToLog(string msg)
    {
        Debug.Log("Game Output: " + msg);
        StreamWriter sw = new StreamWriter(File.Open(outputPath, FileMode.Append));
        sw.WriteLine(msg);
        sw.Close();
    }
}
