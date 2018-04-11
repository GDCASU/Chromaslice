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

    public int playerNumber;
    public List<FillDropDownList> otherLists;

    private List<string> profileList;
    private Dropdown thisList;
    private List<string> displayList;

    private void Awake()
    {
        //Initializes variables
        thisList = GetComponent<Dropdown>();
        profileList = new List<string>();
        displayList = new List<string>();

        //Default element for drop down list
        profileList.Add("Select A Profile");

        //Fills profileNames with the names from profile list in manager
        foreach (Profile profile in GameManager.singleton.profileList)
        {
            profileList.Add(profile.name);
        }

        //Resets the list and then updates the display
        ResetList();
        UpdateDisplay();

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
        //If the selected is a profile
        //Note that value at 0 is the "Select a Profile" tab
        if (change.value != 0)
        {
            //Name of what is currently selected
            string name = displayList[change.value];

            //Makes a new profile based on the selected name
            Profile selected = findProfile(name);

            //Adds the profile to the array of selected profiles
            GameManager.singleton.selectedProfiles[this.playerNumber] = selected;
        }
        else
        {
            //Resets the value of currentSelected
            GameManager.singleton.resetSelected(this.playerNumber);

        }

        //Goes through all other lists and updates them accordingly
        foreach (FillDropDownList list in otherLists)
        {
            //Saves the string of the name that is currently selected
            //Note that this name is for the other lists 
            string name = list.displayList[list.thisList.value];

            //Updates the list and the display
            list.UpdateList();
            list.UpdateDisplay(name);
        }
    }

    /*
     * Resets the displayList to the default values from profileList
     */
    private void ResetList()
    {
        this.displayList = new List<string>();

        foreach(string name in profileList)
        {
            this.displayList.Add(name);
        }
    }

    /**
     * Updates the displayList depending on what has been chosen
     */
    public void UpdateList()
    {
        ResetList();

        //For loop to check each value within the selectedProfiles array in the GameManager
        for(int x = 0; x < 4; x++)
        {
            //If statement to make sure it does not check itself
            if(x != this.playerNumber)
            {
                //If a value was found then it removes the string from the display list
                if (GameManager.singleton.selectedProfiles[x] != null)
                {
                    string name = GameManager.singleton.selectedProfiles[x].name;

                    this.displayList.Remove(name);
                }
            }
        }
    }

    /*
     * Updates the drop down list so that it displays the current list
     */
    private void UpdateDisplay()
    {
        //Clears all the options then fills with items in displayList
        this.thisList.ClearOptions();
        this.thisList.AddOptions(displayList);
    }

    /*
     * Overloads the previous method
     * Makes sure that the current selected value stays selected
     */
    private void UpdateDisplay(string name)
    {
        //Clears all the options then fills with items in displayList
        this.thisList.ClearOptions();
        this.thisList.AddOptions(displayList);

        //Makes sure that the previous name stays the same after it updates the list
        int index = this.displayList.IndexOf(name);
        this.thisList.value = index;
    }

    /**
     * Finds the profile with the specified name within the profileList in the GameManager
     */
    private Profile findProfile(string name)
    {
        //Checks each profile within the profileList and returns the profile with the same name
        foreach(Profile profile in GameManager.singleton.profileList)
        {
            if(profile.name.Equals(name))
            {
                return profile;
            }
        }

        return null;
    }
}
