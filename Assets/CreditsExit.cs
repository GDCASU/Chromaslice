using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SoundManager.singleton.ResetLevelMusic("ChromasliceTheme");
            SceneManager.LoadScene("Title");
        }

		if (Input.GetKeyUp(KeyCode.Space) && Time.timeScale != 2.5f)
		{
			Time.timeScale = 5f;
        }
	}

	void OnDestroy()
	{
		Time.timeScale = 1.0f;
	}

}
