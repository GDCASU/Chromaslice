using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //this needs to be added when working with anything UI related

public class ExitGame : MonoBehaviour
{

    public Button yourButton; //slot the Button object in in the inspector


    void Start()
    {
        Button btn = yourButton.GetComponent<Button>(); // these set up the button functionality
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick() //events in this method are triggered on click
    {
        Application.Quit();
    }
 
}

