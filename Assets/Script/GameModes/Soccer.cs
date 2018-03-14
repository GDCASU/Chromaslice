using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soccer : GameMode
{

	
	protected override void Start ()
    {
        timeRemaining = GameConstants.SoccerTimeLimit;
        base.Start();
	}
	
	
	protected override void Update ()
    {
        base.Update();
	}

    public override void BeginRound()
    {
        timeRemaining = GameConstants.SoccerTimeLimit;
        base.BeginRound();
    }
}
