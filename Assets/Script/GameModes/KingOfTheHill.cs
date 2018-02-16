using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            10/4/2017
// Description:     Rules to determine if game is over for King of the Hill

// Develop:         Nick Arnieri
// Date:            11/3/2017
// Description:     Change behavior of how capping/losing the point works

public class KingOfTheHill : GameMode
{
    public float capTime;
    private bool isContested;

    public float timeTeam0;
    public float timeTeam1;
    public float capTimeTeam0;
    public float capTimeTeam1;
    private bool capturedTeam0;
    private bool capturedTeam1;
    private bool onPointTeam0;
    private bool onPointTeam1;

	// Use this for initialization
	void Start ()
    {
        timeLimit = 30f;
        capTime = 3f;
        timeTeam0 = 0f;
        timeTeam1 = 0f;
        capturedTeam0 = false;
        capturedTeam1 = false;
        onPointTeam0 = false;
        onPointTeam1 = false;
        isContested = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        isContested = onPointTeam0 && onPointTeam1;
        UpdateCapValidation();
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
        if (team == "Team 0")
            onPointTeam0 = true;
        else if (team == "Team 1")
            onPointTeam1 = true;
    }

    /// <summary>
    /// Keeps the cap time values between 0 and 3 for both teams
    /// </summary>
    private void UpdateCapValidation()
    {
        if (capTimeTeam1 < 0)
            capTimeTeam1 = 0;
        else if (capTimeTeam1 > capTime)
            capTimeTeam1 = capTime;

        if (capTimeTeam1 < 0)
            capTimeTeam1 = 0;
        else if (capTimeTeam1 > capTime)
            capTimeTeam1 = capTime;
    }

    /// <summary>
    /// Changes which team is off the point
    /// </summary>
    /// <param name="team">Name of the team that is off the point</param>
    public void ChangeOffPoint(string team)
    {
        if (team == "Team 0")
            onPointTeam0 = false;
        else if (team == "Team 1")
            onPointTeam1 = false;
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
            if (onPointTeam0 && !capturedTeam0)
            {
                // Check if time needs to be added to cap or removed from other teams cap
                if (capTimeTeam1 > 0f)
                {
                    capTimeTeam1 -= Time.deltaTime;
                }
                else
                {
                    capTimeTeam0 += Time.deltaTime;
                }
            }
            else if (onPointTeam1 && !capturedTeam1)
            {
                // Check if time needs to be added to cap or removed from other teams cap
                if (capTimeTeam0 > 0f)
                {
                    capTimeTeam0 -= Time.deltaTime;
                }
                else
                {
                    capTimeTeam1 += Time.deltaTime;
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
            // Only decay cap time if a team cap time is greater then 0 and both teams are off point
            if (!onPointTeam0 && !onPointTeam1)
            {
                if (capTimeTeam0 > 0f)
                {
                    capTimeTeam0 -= Time.deltaTime;
                }
                else if (capTimeTeam1 > 0f)
                {
                    capTimeTeam1 -= Time.deltaTime;
                }
            }
        }
    }

    /// <summary>
    /// Updates which team is in control of the point
    /// </summary>
    private void UpdatePointControl()
    {
        // Only check point control if point isn't contested
        if (!isContested)
        {
            if (capturedTeam0)
            {
                // Only remove cap if cap timer goes to 0 or below
                capturedTeam0 = capTimeTeam0 > 0;
            }
            else
            {
                // Only add cap if cap timer goes to cap time or above
                capturedTeam0 = capTimeTeam0 >= capTime;
            }

            if (capturedTeam1)
            {
                // Only remove cap if cap timer goes to 0 or below
                capturedTeam1 = capTimeTeam1 > 0;
            }
            else
            {
                // Only add cap if cap timer goes to cap time or above
                capturedTeam1 = capTimeTeam1 >= capTime;
            }
        }
    }
   
    /// <summary>
    /// Adds to cap time if a team has captured the point
    /// </summary>
    private void UpdateTeamTime()
    {
        if (capturedTeam0)
            timeTeam0 += Time.deltaTime;
        else if (capturedTeam1)
            timeTeam1 += Time.deltaTime;
    }

    /// <summary>
    /// Checks if game is over based on game rules
    /// </summary>
    public bool IsGameOver()
    {
        bool gameOver = false;

        // Checks teams point time against time limit
        if (timeTeam0 >= timeLimit || timeTeam1 >= timeLimit)
            gameOver = true;

        return gameOver;
    }
}
