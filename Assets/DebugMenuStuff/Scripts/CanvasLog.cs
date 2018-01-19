using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Developer:   Excel Ortega
// Date:        11/3/2017
// Description: The CanvasLog a singleton class used with the LoggerUI prefab to 
//              log message in-game. Add the LoggerUI prefab to any canvas to use
//              it in a scene.

public class CanvasLog : NetworkBehaviour {

    public Text canvasLogText;

    public static CanvasLog instance;

    // Initialize the singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<CanvasLog>();
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        // Clears the canvas log text
        if (Input.GetKeyDown(KeyCode.R))
        {
            canvasLogText.text = "";
        }
    }

    /// <summary>
    /// Pass a formatted string similar to Debug.LogFormat or string.Format.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public void LogFormat(string message, params object[] args)
    {
        Log(string.Format(message, args));
    }

    /// <summary>
    /// Log to the canvas in-game.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isDebugLog"></param>
    public void Log(object message, bool isDebugLog = false)
    {
        canvasLogText.text += "\n" + message.ToString();
        if (isDebugLog)
            Debug.Log(message);
    }

}
