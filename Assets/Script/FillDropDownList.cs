using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Developer:    Nicholas Nguyen
//Date:         3/21/18
//Description:  Fills the drop down lists with profiles
public class FillDropDownList : MonoBehaviour {

    public GameManager manager;

    private Dropdown list;
    private List<string> profileNames;

    private void Awake()
    {
        //Initializes variables
        list = GetComponent<Dropdown>();
        profileNames = new List<string>();

        //Fills profileNames with the names from profile list in manager
        foreach (Profile profile in manager.profileList)
        {
            profileNames.Add(profile.name);
        }

        //Makes sure the drop down list is empty
        list.ClearOptions();

        //Displays the names within the list
        list.AddOptions(profileNames);
    }
}
