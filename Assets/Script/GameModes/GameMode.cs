using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    public float scoreTeam1;
    public float scoreTeam2;
    public int numRounds;
    public float timeLimit;
    public float time;

	// Use this for initialization
	void Start () {
        scoreTeam1 = 0;
        scoreTeam2 = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
