// Developer:   Nizar Kury
// Date:        9/22/2017
// Description: Handling starting the game and transitioning to the appropriate scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleMenu : MonoBehaviour {

    //public Dropdown levelSelect;
    //public Dropdown roundSelect;
    public GameManager gameManager;
    public Toggle[] roundSelect;
    public GameObject[] levelSelect;
    private string selectedLevel;
    private int rounds;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadLevel()
    {
        
        foreach (Toggle t in roundSelect)
        {
            if (t.isOn)
            {
                Debug.Log(t.transform.parent.name);
                rounds = int.Parse(t.transform.parent.name);
                break;
            }
        }
        foreach (GameObject t in levelSelect)
        {
            if (t.GetComponentInChildren<Toggle>().isOn)
            {

                selectedLevel = t.GetComponentInChildren<Text>().text;
                Debug.Log(t.GetComponentInChildren<Text>().text);
                break;
            }
        }
        
        GameManager.singleton.StartGame(selectedLevel + "_Level", rounds);
    }
}
