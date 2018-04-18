using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A janky way to manually fix the navigation for when we switch to the Soccer view mode
public class NavigationController : MonoBehaviour 
{
    public GameObject deathMatchPanel;
    public GameObject soccerPanel;

    public Dropdown gameModeDropdown;
    public Button upArrowBtn;
    public Button startBtn;
    [Header("Soccer Dropdown Nav")]
    public Navigation soccerNavigation;
    [Header("Soccer Start Button Nav")]
    public Navigation startButtonS;
    [Header("Soccer Up Arrow Button Nav")]
    public Navigation upArrowS;
    [Header("Deathmatch Dropdown Nav")]
    public Navigation deathMatchNavigation;
    [Header("Deathmatch Start Button Nav")]
    public Navigation startButtonDM;
    [Header("Deathmatch Up Arrow Button Nav")]
    public Navigation upArrowDM;

    void Start()
    {
        UpdateNavigation();
    }

    public void UpdateNavigation()
    {
        
        if (deathMatchPanel.activeSelf)
        {
            gameModeDropdown.navigation = deathMatchNavigation;
            startBtn.navigation = startButtonDM;
            upArrowBtn.navigation = upArrowDM;
        }
        else if (soccerPanel.activeSelf)
        {
            gameModeDropdown.navigation = soccerNavigation;
            startBtn.navigation = startButtonS;
            upArrowBtn.navigation = upArrowS;
        }
    }

}
