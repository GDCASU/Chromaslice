using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description: If there are any interactables in the game, they should derive this class and implement the required methods.
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: September 15, 2017
 * 
 * Version: 1.0.1
 * Author: Zachary Schmalz
 * Date: September 22, 2017
 * Revisions: Code optimizations
 * 
 * Version 1.1.0
 * Author: Zachary Schmalz
 * Date: September 27, 2017
 * Revisions: Added rigidBody component and Team property
 * 
 * Version 2.0.0
 * Author: Zachary Schmalz
 * Date: January 25, 2018
 * Revisions: Transformed the class into an abstract class to be more designer friendly.
*/

public abstract class Interactable : MonoBehaviour
{
    // Player1 & Player2 are read only. Their values are only set from within this class via the Team property
    protected Team team;
    protected GameObject Player1 { get; private set; }
    protected GameObject Player2 { get; private set; }

    // Team property to automatically set Team and Player members
    public Team Team
    {
        get { return team; }
        set
        {
            team = value;
            Player1 = team.player1;
            Player2 = team.player2;
        }
    }

    // Abstract methods: Methods must be implemented in derived classes of this class
    protected abstract void Start();
    protected abstract void Update();
}