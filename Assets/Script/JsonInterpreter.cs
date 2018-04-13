using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

//Developer:    Nicholas Nguyen
//Date:         2/14/18
//Description:  File meant to read and write data for players
//              within an json file. 
//
//Developer:    Nicholas Nguyen
//Date:         2/23/18
//Description:  Made changes so that each player data is stored in
//              their own json file rather than a single file
//
//Developer:    Nicholas Nguyen
//Date:         3/30/18
//Description:  Changed fileName to use persistant data path in the write to json file

public class JsonInterpreter : MonoBehaviour {

    /*
     * Currently set to test the class
     */
    public void Awake()
    {
        //TestWrite();
        //TestRead();
    }

    /*
     * Method that reads the json file for the specified
     * player and returns it
     */
    public static Player ReadFromJson(string playerName)
    {
        string fileName = Application.persistentDataPath + "/Resources/Profiles/" + playerName + "Data.json";

        //Used try and catch to make sure file is found
        try
        {
            string data = File.ReadAllText(fileName);

            return JsonUtility.FromJson<Player>(data);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        //Return null if no file is found
        return null;
    }

    /*
     * Method that writes the inputted player data into a json file
     * 
     * Creates new file if this is a new player
     */
    public static void WriteToJson(Player player)
    {
        string fileName = Application.persistentDataPath + "/Resources/Profiles/" + player.name + "Data.json";

        CheckFile(fileName);

        string data = JsonUtility.ToJson(player);

        File.WriteAllText(fileName, data);
    }

    /*
     * Method that creates a new file if the file does not exist
     */
    private static void CheckFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            FileStream fs = File.Create(fileName);
            fs.Close();
        }
    }

    /**
     * Method to test the WriteToJson method within the class
     */
    private void TestWrite()
    {
        Player player1 = new Player();
        player1.name = "TestPlayerOne";
        player1.team = 1;

        Player player2 = new Player();
        player2.name = "TestPlayerTwo";
        player2.team = 2;

        WriteToJson(player1);
        WriteToJson(player2);
    }

    /*
     * Method to tet the ReadFromJson method within the class
     */
    private void TestRead()
    {
        Player readPlayer = ReadFromJson("TestPlayerOne");

        Debug.Log("Player name: " + readPlayer.name);
        Debug.Log("Player team: " + readPlayer.team);
    }
}