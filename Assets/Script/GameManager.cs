using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
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

// Developer:   Nick Arnieri
// Date:        10/20/2017
// Description: Switch usage of game rules to DeathMatchRules instead of hard coded determination

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Changed spawning system & controls to work with networking, added documentation and
//              rearranged update method. Need to fix titlescreen-skip functionality

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Turns out this wasn't networked properly, dunno why I thought it was

public class GameManager : NetworkBehaviour
{
    public static GameManager singleton;

    public int firstTeamLayer;
    public GameObject teamPrefab;
    public GameObject[] teams;
    [SyncVar]
    public int team1Score;
    [SyncVar]
    public int team2Score;
    public Vector3[] spawnPoints;
    public int numberOfPlayers;
    public DeathMatchRules deathMatch;
    public KingOfTheHillRules hillRules;
    public int maxRounds;
    public int currentRound;
    public bool gameActive;
    [SyncVar]
    public bool matchStarted = false;
    public bool useTitleScreen;
    [SyncVar]
    public bool countdownOver = false;
    [SyncVar]
    public float countdownTimer;
    public float timeBeforeMatch;

    public string level;

    public Color[,] colorPairs = { { new Color(255, 0, 0), new Color(255, 50, 0) }, { new Color(0, 0, 255), new Color(0, 150, 255) } }; //red, orange, blue, cyan

    //game log path
    private string outputPath;
    //used for knowing which player to spawn
    private int activePlayers;

    // Use this for initialization
    void Awake()
    {
        Debug.Log("GameManager is Awake. NetID: " + GetComponent<NetworkIdentity>().netId);
        //make singleton
        if (singleton)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this);

        //setup logging
        outputPath = Application.dataPath + "/gamelog.txt";
        if (!File.Exists(outputPath)) File.Create(outputPath).Close();
        Debug.Log("Logging match results to: " + outputPath);

        //initialize variables
        deathMatch = GetComponent<DeathMatchRules>();
        //hillRules = GetComponent<KingOfTheHillRules>();
        activePlayers = 0;

        //handle title screen skip in editor (currently unsupported until i get around to fixing it)
        //if (!useTitleScreen)
            //StartGame(SceneManager.GetActiveScene().name, maxRounds);
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownTimer > 0)
            countdownTimer -= Time.deltaTime;
        if (gameActive)
        {
            if (matchStarted)
            {
                if (countdownTimer < 0)
                    countdownOver = true;
                if (NetworkServer.active && deathMatch)
                {
                    if (deathMatch.TimeLimit())
                    {
                        KillTeam(null); //draw
                        WriteToLog("Time ran out, it's a draw");
                    }
                }
            }
            else if (countdownTimer < 1)
            {
                matchStarted = true;

                if (deathMatch)
                {
                    deathMatch.Reset();
                }
            }
        }
    }

    public override void OnStartClient()
    {
        Debug.Log("Spawned on client. NetID: " + GetComponent<NetworkIdentity>().netId);
    }

    /// <summary>
    /// Performs necessary actions when one team is defeated
    /// Adds points, advances current round, returns to title if rounds are over
    /// </summary>
    /// <param name="team">Team that got killed</param>
    [Server]
    public void KillTeam(Team team)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            Team t = teams[i].GetComponent<Team>();
            t.ResetTeam();
            t.RpcResetTeam();
            if (t != team && team != null)
            {
                t.AddPoints();
                if (deathMatch)
                    deathMatch.AddScore(t.name);
                NetManager.GetInstance().SendScoreUpdate();
                WriteToLog(t.name + " won the round with " + deathMatch.time + " seconds remaining");
            }
        }
        currentRound++;
        // added 3 new variables here:
        countdownTimer = timeBeforeMatch;
        matchStarted = false;
        // old boolean variables - Paul
        if (currentRound >= maxRounds)
        {
            string winner = deathMatch.GameWinner();
            if (winner == "")
                WriteToLog("Match over. It's a tie");
            else
                WriteToLog("Match over. Winner: " + winner);

            gameActive = false;
            NetManager.GetInstance().StopHost();
            activePlayers = 0;
        }
    }

    /// <summary>
    /// Sets the spawn points for the given team
    /// Calls SpawnTeam to spawn said team
    /// </summary>
    /// <param name="team">Team number</param>
    /// <param name="spawn1">Spawn point for player 1</param>
    /// <param name="spawn2">Spawn point for player 2</param>
    [Server]
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
    [Server]
    public void SpawnTeam(int num)
    {
        Debug.Log("Spawning team " + num);
        teams[num] = Instantiate(teamPrefab);
        teams[num].name = "Team " + num;
        teams[num].layer = firstTeamLayer + num;
        teams[num].GetComponent<Team>().SetSpawnPoints(spawnPoints[num * 2], spawnPoints[num * 2 + 1]);
        teams[num].GetComponent<Team>().SetColors(colorPairs[num, 0], colorPairs[num, 1]);
        NetworkServer.Spawn(teams[num]);
    }

    [Server]
    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
        teams = new GameObject[num];
        spawnPoints = new Vector3[num * 2];
    }

    [Server]
    public void StartGame()
    {
        //NetworkServer.Spawn(gameObject);
        currentRound = 0;
        gameActive = true;
        // New boolean variable position
        matchStarted = false;
        countdownTimer = timeBeforeMatch;
        countdownOver = false;
        NetManager.GetInstance().ServerChangeScene(level);
        NetworkServer.SendToAll(NetManager.ExtMsgType.StartGame, new NetManager.PingMessage());
        WriteToLog("Starting new game, level: " + level + " out of " + maxRounds + " rounds");
    }

    [Client]
    public void OnStartGame(NetworkMessage netMsg)
    {
        gameActive = true;
        // New boolean variable position
        matchStarted = false;
        countdownTimer = timeBeforeMatch;
        countdownOver = false;
    }

    [Server]
    public GameObject SpawnPlayer(Player ply)
    {
        return teams[ply.playerId/2].GetComponent<Team>().SpawnPlayer(ply);
    }

    public void WriteToLog(string msg)
    {
        Debug.Log("Game Output: " + msg);
        StreamWriter sw = new StreamWriter(File.Open(outputPath, FileMode.Append));
        sw.WriteLine(msg);
        sw.Close();
    }
}
