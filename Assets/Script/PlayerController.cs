using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer:    Ryan Black
//Date:         9/21/2017
//Description:  Sets controls based on number of players and controls player movement and 
//              actions (such as jumping). Jumping limit works when both are in the air
//              but not when one is grounded :( hopefully a simple fix

//Developer:    Kyle Aycock
//Date:         9/21/2017
//Description:  Changed to control only one player instead of both, made axes a variable
//              and added a method to change them per instance

public class PlayerController : MonoBehaviour
{

    //Game attributes
    public float numPlayers = 1; //this is what will be changed to change control schema

    //Player attributes
    public float health = 100;
    public float turnSpeed = 150.0f;
    public float acceleration;
    public float maxSpeed;

    //Jump stuff
    public float jumpPower = 5.0f;
    public int maxJumps = 1; //if we want double jump
    float distanceToGround; //to prevent jumping in the air

    //CONTROLLER CONFIG
    public string verticalAxis;
    public string horizontalAxis;
    string playerOneJumpController;
    KeyCode playerOneJumpKeyboard;
    string playerTwoJumpController;
    KeyCode playerTwoJumpKeyboard;



    void Start()
    {
        distanceToGround = transform.position.y;


        //GetComponent<Rigidbody>().freezeRotation = true; //No tipping here

        //CONTROLLER CONFIG
        if (numPlayers == 1) //Use this for two people one controller too
        {
            //Player One
            playerOneJumpController = "joystick 1 button 8"; //Press down on left joystick
            playerOneJumpKeyboard = KeyCode.Space;

            //Player Two
            playerTwoJumpController = "joystick 1 button 9"; //Press down on right joystick
            playerTwoJumpKeyboard = KeyCode.RightControl;
        }
        else if (numPlayers == 2) //only works with two controllers on one computer right now because no multiplayer
        {
            //Player One
            playerOneJumpController = "joystick 1 button 0"; //"A" button
            playerOneJumpKeyboard = KeyCode.Space;

            //Player Two
            playerTwoJumpController = "joystick 2 button 0"; //"A" button
            playerTwoJumpKeyboard = KeyCode.RightControl;
        }
    }

    void Update()
    {

        float player1x = Input.GetAxis(horizontalAxis) * Time.deltaTime * acceleration;
        float player1z = Input.GetAxis(verticalAxis) * Time.deltaTime * acceleration;

        //Debug.Log("1 " + player1x + " " + player1z);

        if ((Input.GetKeyDown(playerOneJumpController) || Input.GetKeyDown(playerOneJumpKeyboard)) && isGrounded())
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
        }

        //player1.GetComponent<Rigidbody>().transform.Rotate(0, player1x, 0);
        GetComponent<Rigidbody>().AddForce(player1x,0,player1z,ForceMode.Acceleration);
        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSpeed);

        //One human on team, each joystick controls a character OR two humans on team only one controller
        /*if (numPlayers == 1)
        {
            if (player1.tag == "Player1")
            {
                float x = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
                float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

                if ((Input.GetKeyDown(playerOneJumpController) || Input.GetKeyDown(playerOneJumpKeyboard)) && isGrounded())
                {
                    player1.GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
                }

                player1.transform.Rotate(0, x, 0);
                player1.transform.Translate(0, 0, z);
            }
            if (player2.tag == "Player2")
            {
                float x = Input.GetAxis("Horizontal2") * Time.deltaTime * turnSpeed;
                float z = Input.GetAxis("Vertical2") * Time.deltaTime * moveSpeed;

                if ((Input.GetKeyDown(playerTwoJumpController) || Input.GetKeyDown(playerTwoJumpKeyboard)) && isGrounded())
                {
                    player2.GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
                }

                player2.transform.Rotate(0, x, 0);
                player2.transform.Translate(0, 0, z);
            }
        }
        else if (numPlayers == 2) //Two humans on team, each have their own controller, one computer, or split keyboard
        {
            float player1x = Input.GetAxis(horizontalAxis) * Time.deltaTime * turnSpeed;
            float player1z = Input.GetAxis(verticalAxis) * Time.deltaTime * moveSpeed;

            //Debug.Log("1 " + player1x + " " + player1z);

            if ((Input.GetKeyDown(playerOneJumpController) || Input.GetKeyDown(playerOneJumpKeyboard)) && isGrounded())
            {
                player1.GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
            }

            //player1.GetComponent<Rigidbody>().transform.Rotate(0, player1x, 0);
            player1.GetComponent<Rigidbody>().transform.Translate(player1x, 0, player1z);
            float player2x = Input.GetAxis("Horizontal3") * Time.deltaTime * turnSpeed;
            float player2z = Input.GetAxis("Vertical3") * Time.deltaTime * moveSpeed;

            //Debug.Log(player2x + " " + player2z);

            if ((Input.GetKeyDown(playerTwoJumpController) || Input.GetKeyDown(playerTwoJumpKeyboard)) && isGrounded())
            {
                player2.GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
            }

            player2.GetComponent<Rigidbody>().transform.Rotate(0, player2x, 0);
            player2.GetComponent<Rigidbody>().transform.Translate(0, 0, player2z);
        }*/

    }

    public void SetControls(string vertical, string horizontal)
    {
        verticalAxis = vertical;
        horizontalAxis = horizontal;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
}