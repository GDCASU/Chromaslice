using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//Created by Paul G 9/20/17
//Developer:    Paul Gellai
//Date:         9/20/2017
//Description:  Has function for restart button to restart the game.
// Modified 9/27/17: Ensured that booleans for match started and countdowntimer get reset.

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single); // loads current scene
        GameManager.singleton.matchStarted = false;
        GameManager.singleton.countdownTimer = GameManager.singleton.timeBeforeMatch;
        GameManager.singleton.countdownOver = false;
    }

}