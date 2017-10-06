using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//Created by Paul G 9/20/17

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single); // loads current scene
        GameManager.singleton.timer = 60;
    }

}