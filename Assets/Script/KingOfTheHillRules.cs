using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            10/4/2017
// Description:     Rules to determine if game is over for King of the Hill

public class KingOfTheHillRules : MonoBehaviour
{
    public float timeLimit;
    public float capTime;
    private bool isContested;

    private float timeTeam1;
    private float timeTeam2;
    private float capTimeTeam1;
    private float capTimeTeam2;
    private bool capturedTeam1;
    private bool capturedTeam2;
    private bool onPointTeam1;
    private bool onPointTeam2;
    private float capDecayTeam1;
    private float capDecayTeam2;

	// Use this for initialization
	void Start ()
    {
        timeLimit = 30f;
        capTime = 3f;
        timeTeam1 = 0f;
        timeTeam2 = 0f;
        capturedTeam1 = false;
        capturedTeam2 = false;
        onPointTeam1 = false;
        onPointTeam2 = false;
        isContested = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        isContested = onPointTeam1 && onPointTeam2;
        UpdateCapTime();
        UpdateCapDecay();
        UpdatePointControl();
        UpdateTeamTime();
	}

    /// <summary>
    /// Changes which team is on the point
    /// </summary>
    /// <param name="team">Name of the team that is on the point</param>
    public void ChangeOnPoint(string team)
    {
        if (team == "Team 1")
            onPointTeam1 = true;
        else if (team == "Team 2")
            onPointTeam2 = true;
    }

    /// <summary>
    /// Changes which team is off the point
    /// </summary>
    /// <param name="team">Name of the team that is off the point</param>
    public void ChangeOffPoint(string team)
    {
        if (team == "Team 1")
            onPointTeam1 = false;
        else if (team == "Team 2")
            onPointTeam2 = false;
    }

    /// <summary>
    /// Updates the capture time of which team is capturing the point
    /// </summary>
    private void UpdateCapTime()
    {
        // Only change cap time if point isn't contested
        if (!isContested)
        {
            // Only increase cap time if the point isn't already captured by the team
            if (onPointTeam1 && !capturedTeam1)
            {
                // Check if time needs to be added to cap or removed from other teams cap
                if (capTimeTeam2 > 0f)
                    capTimeTeam2 -= Time.deltaTime;
                else
                {
                    capTimeTeam1 += Time.deltaTime;
                    capTimeTeam2 = 0f;
                }
            }
            else if (onPointTeam2 && !capturedTeam2)
            {
                // Check if time needs to be added to cap or removed from other teams cap
                if (capTimeTeam1 > 0f)
                    capTimeTeam1 -= Time.deltaTime;
                else
                {
                    capTimeTeam2 += Time.deltaTime;
                    capTimeTeam1 = 0f;
                }
            }
        }
    }

    /// <summary>
    /// Checks if a team has been off the point long enough for their cap time to decay
    /// </summary>
    private void UpdateCapDecay()
    {
        // Only check cap decay if point isnt controlled
        if (!isContested)
        {
            // Only check decay if a team cap time is greater then 0 and off point.
            if (capTimeTeam1 > 0f && !onPointTeam1)
            {
                if (capDecayTeam1 < 3)
                    capDecayTeam1 += Time.deltaTime;
                else
                {
                    capTimeTeam1 -= Time.deltaTime;

                    // Reset values when fully decayed
                    if (capTimeTeam1 < 0f)
                    {
                        capTimeTeam1 = 0f;
                        capDecayTeam1 = 0f;
                    }
                }
            }
            else
            {
                capDecayTeam1 = 0f;
            }

            if (capTimeTeam2 > 0f && !onPointTeam2)
            {
                if (capDecayTeam2 < 3)
                    capDecayTeam2 += Time.deltaTime;
                else
                {
                    capTimeTeam2 -= Time.deltaTime;

                    // Reset values when fully decayed
                    if (capTimeTeam2 < 0f)
                    {
                        capTimeTeam2 = 0f;
                        capDecayTeam2 = 0f;
                    }
                }
            }
            else
            {
                capDecayTeam2 = 0f;
            }
        }
    }

    /// <summary>
    /// Updates which team is in control of the point
    /// </summary>
    private void UpdatePointControl()
    {
        // Only check point control is point isn't contested
        if (!isContested)
        {
            if (capTimeTeam1 >= capTime)
            {
                capturedTeam1 = true;
                capTimeTeam1 = 0f;
            }
            else if (capTimeTeam2 >= capTime)
            {
                capturedTeam2 = true;
                capTimeTeam2 = 0f;
            }
        }
    }
   
    /// <summary>
    /// Adds to cap time if a team has captured the point
    /// </summary>
    private void UpdateTeamTime()
    {
        if (capturedTeam1)
            timeTeam1 += Time.deltaTime;
        else if (capturedTeam2)
            timeTeam2 += Time.deltaTime;
    }

    /// <summary>
    /// Checks if game is over based on game rules
    /// </summary>
    public bool IsGameOver()
    {
        bool gameOver = false;

        // Checks teams point time against time limit
        if (timeTeam1 >= timeLimit || timeTeam2 >= timeLimit)
            gameOver = true;

        return gameOver;
    }
}
