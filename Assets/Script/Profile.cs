using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer:    Nicholas Nguyen
//Date:         3/16/18
//Description:  Made the basic setup of the profile class consisting
//              of the player, name, a constructor, and retrieving 
//              the player from a json file

public class Profile {

    public Player player;
    public string name;

    /**
     * Retrieves the player object from the json file
     */
    private void getPlayer()
    {
        player = JsonInterpreter.ReadFromJson(name);
    }


}
