using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created by Paul G 9/16/17

//Kyle Aycock 11/17/17 - Changed score fetching to play nicer with networking

public class CanvasScript : MonoBehaviour
{
    public GameObject debugMenu;
    public Sprite[] numberSet;

    public Image team1Score;
    public Image team2Score;
    public Image minutes;
    public Image tensSeconds;
    public Image seconds;

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

        // Update the timer and team scores if the game is active
        if (GameManager.singleton.currentGame != null && GameManager.singleton.currentGame.IsGameActive)
        {
            float time = GameManager.singleton.currentGame.TimeReamining;
            minutes.sprite = numberSet[(int)time / 60];
            tensSeconds.sprite = numberSet[(int)((time % 60) / 10)];
            seconds.sprite = numberSet[(int)((time % 60) % 10)];

            team1Score.sprite = numberSet[(int)GameManager.singleton.currentGame.Team1Score];
            team2Score.sprite = numberSet[(int)GameManager.singleton.currentGame.Team2Score];
        }

        else
        {
            //timer.text = Mathf.FloorToInt(GameManager.singleton.currentGame.TimeReamining).ToString();
        }
    }
}
