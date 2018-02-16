using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

//Developer:    Nicholas Nguyen
//Date:         2/14/18
//Description:  File meant to read and write data for players
//              within an json file. 

public class JsonInterpreter : MonoBehaviour {

    public List<Player> playerList;

    private string data;
    private string fileName;

    /*
     * Currently set to test the class
     */
    public void Awake()
    {
        playerList = new List<Player>();
        TestWrite();
        TestRead();
    }

    /*
     * Method that reads the json file containing the player data
     * makes a COMPLETELY NEW PLAYER LIST
     */
    private void CreatePlayerList()
    {
        fileName = Application.dataPath + "/Resources/PlayerData.json";

        CheckFile();

        data = File.ReadAllText(fileName);
        playerList = new List<Player>();

        //array of strings is used to seperate each player data
        string[] playerString = data.Split('|');

        foreach(string player in playerString)
        {
            Player playerObject = JsonUtility.FromJson<Player>(player);
            playerList.Add(playerObject);
        }
    }

    /*
     * Method that writes the playerList data into
     * the json file
     * 
     * In order to store all the players within
     * a single Json, the string being written into the jfile
     * each individual player is split up between a '|'
     */
    private void Write()
    {
        fileName = Application.dataPath + "/Resources/PlayerData.json";

        CheckFile();

        data = "";

        foreach(Player player in playerList)
        {
            data += JsonUtility.ToJson(player) + "|";
        }

        //This is used to remove the last '|' that occurs
        data = data.Substring(0, data.Length - 1);

        File.WriteAllText(fileName, data);
    }

    /*
     * Method that makes sure the file can be found
     */
    private void CheckFile()
    {
        try
        {
            if (!File.Exists(fileName))
            {
                FileStream fs = File.Create(fileName);
                fs.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exeption: " + e);
        }
    }

    /**
     * Method to test the writing method within the class
     */
    private void TestWrite()
    {
        Player player1 = new Player();
        player1.name = "Krabs";
        player1.team = 1;

        Player player2 = new Player();
        player2.name = "Boofboi";
        player2.team = 2;

        playerList.Add(player1);
        playerList.Add(player2);
        Debug.Log(playerList);
        Write();
    }

    /*
     * Method to tet the reading method within the class
     */
    private void TestRead()
    {
        CreatePlayerList();

        foreach(Player player in playerList)
        {
            Debug.Log(player.name);
        }
    }
}