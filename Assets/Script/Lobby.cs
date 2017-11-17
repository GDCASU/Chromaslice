using MaterialUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Handles I/O for the online lobby

public class Lobby : MonoBehaviour {

    public NetManager nm;

    public InputField player1Name;
    public InputField player2Name;
    public bool twoPlayers;
    public SelectionBoxConfig level;
    public SelectionBoxConfig rounds;
    public InputField ipField;

    public Text[] playerNameTexts;
    public Button startButton;
    public Text playerText;

    private bool started = false;

    // Use this for initialization
    void Start () {
        nm = NetManager.GetInstance();
	}

    public void SetPlayers(bool two)
    {
        twoPlayers = two;
    }

    public void StartHost()
    {
        if (!started)
        {
            UpdateInfo();
            nm.StartHost();
            started = true;
        }
    }

    public void JoinGame()
    {
        if (!started)
        {
            UpdateInfo();
            nm.networkAddress = ipField.text;
            nm.StartClient();
            started = true;
        }
    }

    public void UpdateInfo()
    {
        nm.localPlayers.Add(new Player() { name = player1Name.text, controllerId = 0 });
        if(twoPlayers)
            nm.localPlayers.Add(new Player() { name = player2Name.text, controllerId = 1 });
    }

    public void UpdatePlayerDisplay()
    {
        List<Player> list = NetManager.GetInstance().playerList;
        if (list.Count > 4)
            Debug.LogWarning("More than 4 players somehow!");
        for (int i = 0; i < list.Count; i++)
            playerNameTexts[i].text = list[i].name;
        if (list.Count == 4)
        {
            startButton.interactable = true;
            playerText.text = "Ready to Start!";
        }
        else
            startButton.interactable = false;
    }

    //only callable by host = server
    public void StartGame()
    {
        GameManager.singleton.StartGame(level.selectedText.text + "_Level", rounds.currentSelection + 1);
    }

    public void Exit()
    {
        NetManager.GetInstance().StopHost();
        SceneManager.LoadScene(0);
    }
}
