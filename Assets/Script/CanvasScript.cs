using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created by Paul G 9/16/17

//Kyle Aycock 11/17/17 - Changed score fetching to play nicer with networking

public class CanvasScript : MonoBehaviour
{
    public Text teamScoreText1;
    public Text tesmScoreText2;
    public Text timer;
    public Text speedp1;
    public Text speedp2;
    public Text points;
    public GameObject debugMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("Key was pressd");
            //debugMenu.SetActive(!debugMenu.activeSelf);
        }

        if (GameManager.singleton.currentGame != null && GameManager.singleton.currentGame.IsGameActive)
        {
            float time = GameManager.singleton.currentGame.TimeReamining;
            timer.text = Mathf.FloorToInt(time / 60).ToString("00") + ":" + Mathf.FloorToInt(time % 60).ToString("00");

            teamScoreText1.text = GameManager.singleton.currentGame.Team1Score.ToString();
            tesmScoreText2.text = GameManager.singleton.currentGame.Team2Score.ToString();
        }

        else
        {
            timer.text = Mathf.FloorToInt(GameManager.singleton.currentGame.TimeReamining).ToString();
        }
    }
}
