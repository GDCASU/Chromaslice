using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        8/28/17
// Description: Simple class to process user input to move the player

// Date: 9/21/17, class is obsolete, will be removed soon

public class PlayerInput : MonoBehaviour
{
    public string vertInput;
    public string horizInput;
    public float speed;

    // Use this for initialization
    void Start()
    {
        if (transform.parent.name == "Team 2")
        {
            if (name == "Player 1")
            {
                vertInput = "Vertical3";
                horizInput = "Horizontal3";
            }
            else
            {
                vertInput = "Vertical4";
                horizInput = "Horizontal4";
            }
        }

        gameObject.layer = transform.parent.gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (transform.forward * Input.GetAxis(vertInput) + transform.right * Input.GetAxis(horizInput)) * speed * Time.deltaTime;
    }


}
