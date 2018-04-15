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

//Developer:    Nicholas Nguyen
//Date:         3/16/18
//Description:  Added the profile list and had it initialized within the Awake
//              by using new method called createProfiles

//Developer:    Nicholas Nguyen
//Date:         3/30/18
//Description:  Small addition of profile array for the one's 
//              that are currently selected. Methods for this were also added

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public GameObject teamPlayerPrefab;
    public GameObject teamAiPrefab;
    public GameObject[] teams;
    public GameMode currentGame;

    public Color[,] colorPairs =
		{ 
			{ new Color(255, 0, 0), new Color(255, 0, 180) },
			{ new Color(0, 0, 255), new Color(0, 255, 150) }
		}; //red, magenta, blue, greenblue
    public string level;

    public List<Profile> profileList;
    public Profile[] selectedProfiles;

    //public bool useTitleScreen;


    private int numberOfPlayers;
    private Vector3[] spawnPoints;
    private int firstTeamLayer;
    private string outputPath;
    public int activePlayers;
    private string filename;

    // Use this for initialization
    void Start()
    {
        //make singleton
        if (singleton)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(gameObject);

        //setup logging
        outputPath = Application.dataPath + "/gamelog.txt";
        if (!File.Exists(outputPath)) File.Create(outputPath).Close();
        Debug.Log("Logging match results to: " + outputPath);

        activePlayers = 0;

        //Initializes the profile list and then fills it
        profileList = new List<Profile>();
        createProfiles();

        //Initializes the array of selected profiles
        //Chose 4 for 4 players
        selectedProfiles = new Profile[4];

        SetGameMode(typeof(Deathmatch));

        //handle title screen skip in editor (currently unsupported until i get around to fixing it)
        //if (!useTitleScreen)
            //StartGame(SceneManager.GetActiveScene().name, maxRounds);
    }

    public void KillTeam(GameObject player)
    {
        currentGame.KillTeam(player);
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
        teams[num] = Instantiate(teamPlayerPrefab);
        teams[num].name = "Team " + num;
        teams[num].layer = firstTeamLayer + num;
        teams[num].GetComponent<Team>().SetSpawnPoints(spawnPoints[num * 2], spawnPoints[num * 2 + 1]);
        teams[num].GetComponent<Team>().SetColors(colorPairs[num, 0], colorPairs[num, 1]);
        teams[num].GetComponent<Team>().SetTeamRope(num);
        NetworkServer.Spawn(teams[num]);
        NetManager.GetInstance().SpawnReadyPlayers(num);
    }

    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
        teams = new GameObject[num];
        spawnPoints = new Vector3[num * 2];
    }

    public void StartGame()
    {
        //NetworkServer.Spawn(gameObject);
        currentGame.BeginRound();
        NetManager.GetInstance().ServerChangeScene(level);
        NetworkServer.SendToAll(NetManager.ExtMsgType.StartGame, new NetManager.PingMessage());
        WriteToLog("Starting new game, level: " + level + " out of " + currentGame.GameRoundLimit + " rounds");
    }

    public void OnStartGame(NetworkMessage netMsg)
    {
        currentGame.BeginRound();
    }

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

    // Set the current game mode to be played (Deathmatch is default)
    public void SetGameMode(System.Type mode)
    {
        Destroy(GetComponent<GameMode>());
        currentGame = (GameMode)gameObject.AddComponent(mode);
    }

    /**
     * Fills the profile list with the json files found
     * within the profile folder found within the resources folder
     */
    private void createProfiles()
    {
        //Directory where the profiles are stored
        filename = Application.persistentDataPath + "/Resources/Profiles/";
        DirectoryInfo d = new DirectoryInfo(filename);

        if (!d.Exists)
            d.Create();

        //Array of the files within the directory
        FileInfo[] files = d.GetFiles();
        //For each of the files it creates a new profile and stores it within the player list
        foreach(FileInfo file in files)
        {
            //File directory of the profile
            string playerFileName = file.ToString();

            //If the file is a json file
            if (playerFileName.EndsWith(".json"))
            {
                //These find the profile name from the directory
                int nameStartIndex = playerFileName.LastIndexOf("\\") + 1;
                string playerName = playerFileName.Substring(nameStartIndex);
                playerName = playerName.Substring(0, playerName.Length - 9);

                //Creates a new profile and adds it to the list
                Profile newProfile = new Profile();
                newProfile.name = playerName;
                profileList.Add(newProfile);
            }
        }
    }

    /**
     * Method that adds a profile to array of selected profiles
     */
    public void addSelected(Profile profile, int index)
    {
        this.selectedProfiles[index] = profile;
    }
    
    /**
     * Resets the Profile within the selectedProfiles array
     * by making it null
     */
    public void resetSelected(int index)
    {
        this.selectedProfiles[index] = null;
    }

    // Mainly for switching to Credits
    public void SwitchScene(string scene)
    {
        SoundManager.singleton.ResetLevelMusic(scene, 1);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}