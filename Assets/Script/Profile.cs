using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer:    Nicholas Nguyen
//Date:         3/16/18
//Description:  Made the basic setup of the profile class consisting
//              of the player, name, a constructor, and retrieving 
//              the player from a json file

public class Profile : MonoBehaviour {

    public Player player;
    public string name;

    JsonInterpreter interpreter;

    /**
     * Constructor for the profile to initialize the name
     * <param name="name">The name of the profile</param>
     */
    public Profile(string name)
    {
        this.name = name;
    }

    private void Awake()
    {
        //Initializes the JsonInterpreter
        interpreter = GetComponent<JsonInterpreter>();
    }

    /**
     * Retrieves the player object from the json file
     */
    private void getPlayer()
    {
        player = interpreter.ReadFromJson(name);
    }
}
