// Developer:   Nizar Kury
// Date:        9/22/2017
// Description: Handling starting the game and transitioning to the appropriate scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleMenu : MonoBehaviour {

    public Dropdown levelSelect;
    public Dropdown roundSelect;
    public GameManager gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel()
    {
        string selectedLevel = levelSelect.transform.Find("Label").GetComponent<Text>().text;
        int rounds = roundSelect.value + 1; // add 1 because 0 is the first option
        GameManager.singleton.StartGame(selectedLevel + "_Level", rounds);
    }
}
