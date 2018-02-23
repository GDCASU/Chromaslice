// Developer:   Nizar Kury
// Date:        9/22/2017
// Description: Handling starting the game and transitioning to the appropriate scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleMenu : MonoBehaviour {

    public Toggle[] roundSelect;
    public GameObject[] levelSelect;
    public GameObject EliminationPanel;
    public GameObject DeathmatchPanel;
    public GameObject SoccerPanel;

    private string selectedLevel;
    private int rounds;

    public void StartLocal()
    {
        
        foreach (Toggle t in roundSelect)
        {
            if (t.isOn)
            {
                Debug.Log(t.transform.parent.name);
                rounds = int.Parse(t.transform.parent.name);
                break;
            }
        }
        foreach (GameObject t in levelSelect)
        {
            if (t.GetComponentInChildren<Toggle>().isOn)
            {
                selectedLevel = t.GetComponentInChildren<Text>().text;
                Debug.Log(t.GetComponentInChildren<Text>().text);
                break;
            }
        }

        NetManager.GetInstance().StartLocalGame();
        GameManager.singleton.level = selectedLevel + "_Level";
        GameManager.singleton.maxRounds = rounds;
        GameManager.singleton.StartGame();
    }

    public void StartOnline()
    {
        NetManager.GetInstance().ServerChangeScene(NetManager.GetInstance().lobbySceneOffline);
    }

    public void SelectGamemode(int option)
    {
        EliminationPanel.SetActive(option == 0);
        DeathmatchPanel.SetActive(option == 1);
        SoccerPanel.SetActive(option == 2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
