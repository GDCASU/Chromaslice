using UnityEngine;
using UnityEngine.UI;

// Developer:   Excel Ortega
// Date:        11/3/2017
// Description: The CanvasLog is a singleton class used with the InGameDebugLoggerWindow prefab to 
//              log message in-game. Add the prefab to any canvas to start using it in a scene.

//              Once added in a scene, you can use the prefab by calling CanvasLog.instance.Log(...) 
//              or CanvasLog.instance.LogFormat(...)

public class CanvasLog : MonoBehaviour {

    public GameObject window;
    public Text canvasLogText;
    public bool DontDestroy;

    public static CanvasLog instance;
    private bool isShowing;

    // Initialize the singleton
    private void Awake()
    {
        if (DontDestroy)
            DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = GetComponent<CanvasLog>();
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        isShowing = false;

        window.SetActive(isShowing);
    }

    private void Update()
    {
        // Clears the canvas log text
        if (Input.GetKeyDown(KeyCode.R))
        {
            canvasLogText.text = "";
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            isShowing = !isShowing;

            window.SetActive(isShowing);
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
