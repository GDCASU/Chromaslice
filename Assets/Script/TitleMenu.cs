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

    public Toggle[] roundSelect;
    public GameObject[] deathmatchLevelSelect;
    public GameObject[] soccerLevelSelect;
    public GameObject DeathmatchPanel;
    public GameObject SoccerPanel;

    private string selectedLevel;
    private int rounds;

    public void Start()
    {
        // Deathmatch as default level
        SelectGamemode(0);
    }

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

    public void ExitGame()
    {
        Application.Quit();
    }
}
