// Developer:   Nizar Kury
// Date:        9/22/2017
// Description: Handling starting the game and transitioning to the appropriate scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleMenu : MonoBehaviour
{

    //local game
    public Toggle[] roundSelect;
    public GameObject[] deathmatchLevelSelect;
    public GameObject[] soccerLevelSelect;
    public GameObject DeathmatchPanel;
    public GameObject SoccerPanel;

    //controls
    public Dropdown selectedPlayer;
    public Text playerText;
    public Button dashButton;
    public Button powerUpButton;
    public Dropdown selectedMovement;

    private string selectedLevel;
    private int rounds;

    public void StartLocal()
    {
        // Select number of rounds
        foreach (Toggle t in roundSelect)
        {
            if (t.isOn)
            {
                rounds = int.Parse(t.transform.parent.name);
                break;
            }
        }

        // Select levels from deathmatch panel
        if (DeathmatchPanel.activeSelf)
        {
            foreach (GameObject t in deathmatchLevelSelect)
            {
                if (t.GetComponentInChildren<Toggle>().isOn)
                {
                    selectedLevel = t.GetComponentInChildren<Text>().text;
                    Debug.Log(t.GetComponentInChildren<Text>().text);
                    break;
                }
            }
        }

        // Select levels from soccer level
        else if(SoccerPanel.activeSelf)
        {
            foreach (GameObject t in soccerLevelSelect)
            {
                if (t.GetComponentInChildren<Toggle>().isOn)
                {
                    selectedLevel = t.GetComponentInChildren<Text>().text;
                    Debug.Log(t.GetComponentInChildren<Text>().text);
                    break;
                }
            }
        }

        NetManager.GetInstance().StartLocalGame();
        GameManager.singleton.level = selectedLevel + "_Level";
        GameManager.singleton.currentGame.GameRoundLimit = rounds;
        GameManager.singleton.StartGame();

        SoundManager.singleton.PlayLevelMusic(GameManager.singleton.level);
    }

    public void StartOnline()
    {
        NetManager.GetInstance().ServerChangeScene(NetManager.GetInstance().lobbySceneOffline);
    }

    // Updates the gamemode selected via the UI
    public void SelectGamemode(int option)
    {
        DeathmatchPanel.SetActive(option == 0);
        SoccerPanel.SetActive(option == 1);

        switch (option)
        {
            case 0:
                GameManager.singleton.SetGameMode(typeof(Deathmatch));
                break;

            case 1:
                GameManager.singleton.SetGameMode(typeof(Soccer));
                break;

            default :
                GameManager.singleton.SetGameMode(typeof(Deathmatch));
                break;
        }

    }

    public void PopulateControls()
    {
        int player = selectedPlayer.value + 1;
        playerText.text = "Player " + player;
        Controls c = Controls.LoadFromConfig(player);
        dashButton.GetComponentInChildren<Text>().text = c.dashButton.keyPos.ToString();
        powerUpButton.GetComponentInChildren<Text>().text = c.powerUpButton.keyPos.ToString();
        if (c.horizAxis.axis.EndsWith(".5"))
            selectedMovement.value = 1;
        else if (c.horizAxis.axis.EndsWith(".8"))
            selectedMovement.value = 2;
        else
            selectedMovement.value = 0;
    }

    public void ChangeDashButton()
    {
        StartCoroutine(ChangeControl(selectedPlayer.value, true));        
    }

    public void ChangePowerupButton()
    {
        StartCoroutine(ChangeControl(selectedPlayer.value, false));
    }

    public void ChangeMovement()
    {
        int player = selectedPlayer.value + 1;
        Controls c = Controls.LoadFromConfig(player);
        switch(selectedMovement.value)
        {
            case 0:
                c.horizAxis = new Controls.ControlInput("Horizontal" + player);
                c.vertAxis = new Controls.ControlInput("Vertical" + player);
                break;
            case 1:
                c.horizAxis = new Controls.ControlInput("Horizontal" + player + ".5");
                c.vertAxis = new Controls.ControlInput("Vertical" + player + ".5");
                break;
            case 2:
                c.horizAxis = new Controls.ControlInput("Horizontal" + player + ".8");
                c.vertAxis = new Controls.ControlInput("Vertical" + player + ".8");
                break;
        }

        Controls.SaveToConfig(c);
    }

    public IEnumerator ChangeControl(int player, bool dash)
    {
        if (dash)
        {
            dashButton.interactable = false;
            dashButton.GetComponentInChildren<Text>().text = "Press a button";
        }
        else
        {
            powerUpButton.interactable = false;
            powerUpButton.GetComponentInChildren<Text>().text = "Press a button";
        }
        Debug.Log("Waiting for input...");
        Controls c = Controls.LoadFromConfig(player+1);
        Controls.ControlInput input;
        if (dash)
            input = c.dashButton;
        else
            input = c.powerUpButton;
        yield return StartCoroutine(Controls.WaitForInput(input));
        Debug.Log("Found input. Saving...");
        Controls.SaveToConfig(c);
        if (dash)
            dashButton.interactable = true;
        else
            powerUpButton.interactable = true;
        PopulateControls();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
