using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneSwitcher : MonoBehaviour 
{

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && Input.GetKeyDown(KeyCode.JoystickButton3) && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SoundManager.singleton.ResetLevelMusic("Title", 0);
            SceneManager.LoadScene("Title");
        }
	}
}
