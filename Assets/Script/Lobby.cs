using MaterialUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

    public NetManager nm;

    public InputField player1Name;
    public InputField player2Name;
    public bool twoPlayers;
    public SelectionBoxConfig level;
    public SelectionBoxConfig rounds;
    public InputField ipField;

    public Text[] playerNameTexts;

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
        for (int i = 0; i < NetManager.GetInstance().playerList.Count; i++)
            playerNameTexts[i].text = NetManager.GetInstance().playerList[i].name;
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
