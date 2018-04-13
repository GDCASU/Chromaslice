using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Developer:   Trevor Angle
// Date:        4/13/2018
// Description: Displays results screen with winner and point
//              totals. Has an adjustable delay before displaying,
//              rather than displaying immediately when one team
//              wins.

public class EndDisplay : MonoBehaviour {
    //Public fields
    public float displayDuration;
    public float delayDuration;
    public string team1WinText;
    public string team2WinText;
    public string tieText;

    //Public properties
    public int Team1Score { set { team1Score = value; } }
    public int Team2Score { set { team2Score = value; } }

    //Private fields
    private Text endText;
    private int team1Score;
    private int team2Score;
    private float displayStartTime;
    private bool shouldBeDisplaying;
    private GameMode gameMode;


    // Use this for initialization
    void Start () {
        //Private initialization
        endText = gameObject.GetComponent<Text>();
        shouldBeDisplaying = false;
        gameMode = GameManager.singleton.currentGame;

        //Start invisible
        endText.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        team1Score = (int)gameMode.Team1Score;
        team2Score = (int)gameMode.Team2Score;

        //if the game just ended
        if (!shouldBeDisplaying && gameMode.IsGameOver) {
            StartCoroutine("DelayDisplay");
        }
    }

    // Delays the displaying of the results for delayDuration amount of time
    private IEnumerator DelayDisplay() {
        yield return new WaitForSeconds(delayDuration);
        Display();
    }

    // Displays the end text for displayDuration amount of time
    private void Display() {
        //if we didn't already start displaying and we haven't finished displaying
        if (!shouldBeDisplaying && !DoneDisplaying()) {
            if (team1Score > team2Score) {
                endText.text = team1WinText;
                endText.color = Color.red;
            }
            else if (team2Score > team1Score) {
                endText.text = team2WinText;
                endText.color = Color.blue;
            }
            else {
                endText.text = tieText;
                endText.color = Color.white;
            }
            endText.text += "\nRed score: " + team1Score +
                           "\nBlue score: " + team2Score;
            endText.enabled = true;
            shouldBeDisplaying = true;
            displayStartTime = Time.time;
        }
    }

    //This helps keep the display from being shown multiple times
    private Boolean DoneDisplaying() {
        return shouldBeDisplaying && Time.time - displayStartTime >= displayDuration;
    }
}
