using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Developer:    Nicholas Nguyen
//Date:         3/21/18
//Description:  Fills the drop down lists with profiles

//Developer:    Nicholas Nguyen
//Date:         3/28/18
//Description:  Updates the game manager which profiles are selected

public class FillDropDownList : MonoBehaviour
{

    public GameManager manager;
    public int playerNumber;
    public List<FillDropDownList> otherLists;
    public string currentSelected;

    private List<string> profileList;
    private Dropdown thisList;
    private List<string> selectedNames;

    private void Awake()
    {
        //Initializes variables
        thisList = GetComponent<Dropdown>();
        profileList = new List<string>();
        currentSelected = "none";
        selectedNames = new List<string>();

        //Default element for drop down list
        profileList.Add("Select A Profile");

        //Fills profileNames with the names from profile list in manager
        foreach (Profile profile in manager.profileList)
        {
            profileList.Add(profile.name);
        }

        //Fills the drop down list with profiles
        thisList.ClearOptions();
        thisList.AddOptions(profileList);

        //Adds the listener for when the drop down value is changed
        thisList.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(thisList);
        });
    }

    /**
     * Method for when the player chooses a profile
     */
    private void DropdownValueChanged(Dropdown change)
    {
        //Finds what names are currently selected
        selectedNames = new List<string>();
        foreach(FillDropDownList list in this.otherLists)
        {
            selectedNames.Add(list.currentSelected);
        }

        //If the selected is a profile
        //Note that value at 0 is the "Select a Profile" tab
        if (change.value != 0)
        {
            //Retrieves the selected profile from the list of profiles
            //change.value is being subtracted to compensate for the default element
            Profile selected = manager.profileList[change.value - 1];

            //If the name is not currently selected
            if(!selectedNames.Contains(selected.name))
            {
                //Adds the profile to the array of selected profiles
                manager.addSelected(selected, playerNumber);
                //Sets the selected profile as the current one
                this.currentSelected = selected.name;

                //Debug.Log("Profile Added");
            }
            //If the name has been selected
            else
            {
                //Sets the list value to the default tab
                thisList.value = 0;

                //Debug.Log("Profile not added");
            }
        }
        //If the default tab is selected
        else
        {
            //Resets the selected profile within the game manager
            manager.resetSelected(this.playerNumber);
            //Sets the current selected value to "none"
            this.currentSelected = "none";

            //Debug.Log("Default Profile set");
        }
    }
}
