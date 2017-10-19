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

//Developer:    Ryan Black
//Date:         9/27/2017
//Description:  Fixed jumping, exposed more movement attributes for designers, and added a dash function. Right now the dash function is more of a "sprint", a two second burst in speed,
//              and it applies team-wide instead of to the specific cube. This is not intended but gets the point across and I can't seem to fix it. Pressing down on right/left stick
//              activates the dash for the team, and the dash lasts for dashTime, which is currently two seconds. It isn't mapped to the triggers because the triggers are an axis and
//              not a simple button like the shoulder buttons or the joystick buttons, and I couldn't figure out how to impliment them.

/*
 *Developer:      Zachary Schmalz
 *Date:           9/29/2017
 *Description:    Added a property for changing the max speed of the players because the speed
 *                PowerUp was not actually affecting the max speed.
 */

/*
 *Developer:      Zachary Schmalz
 *Date:           10/4/2017
 *Description:    Added buttons/keys for activating a PowerUp that is currently held by the team
 */

/*
*Developer:      Ryan Black
*Date:           10/11/2017
*Description:    Added modifiable deceleration
*/

public class PlayerController : MonoBehaviour
{

    //Game attributes
    public float numPlayers = 1; //is this even needed anymore?

    //Player attributes
    public float health = 100;
    public float turnSpeed = 150.0f;
    public float acceleration;
    public float maxSpeed;
    public float friction = 0; //0 - 1
    public float dashPower = 5;
    public float dashTime = 2;
    public float decelerationRate = 0.025f; // 0 - 1
    public int team;

    private float tempDashPower;
    private float tempMaxSpeed;

    private bool dashing = false;
    private float timeSinceDash = 0;

    // Property for affecting the maxSpeed of the players
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set
        {
            maxSpeed = value;
            tempMaxSpeed = maxSpeed;
        }
    }

    //Jump stuff
    public float jumpPower = 25.0f;
    public int maxJumps = 1; //if we want double jump
    float distanceToGround; //to prevent jumping in the air

    //CONTROLLER CONFIG
    public string verticalAxis;
    public string horizontalAxis;

    string playerOneJumpController;
    KeyCode playerOneJumpKeyboard;
    string playerOneDashController;
    KeyCode playerOneDashKeyboard;
    string playerOnePowerUpController;
    KeyCode playerOnePowerUpKeyboard;

    string playerTwoJumpController;
    KeyCode playerTwoJumpKeyboard;
    string playerTwoDashController;
    KeyCode playerTwoDashKeyboard;
    string playerTwoPowerUpController;
    KeyCode playerTwoPowerUpKeyboard;

    string playerThreeJumpController;
    KeyCode playerThreeJumpKeyboard;
    string playerThreeDashController;
    KeyCode playerThreeDashKeyboard;
    string playerThreePowerUpController;
    KeyCode playerThreePowerUpKeyboard;

    string playerFourJumpController;
    KeyCode playerFourJumpKeyboard;
    string playerFourDashController;
    KeyCode playerFourDashKeyboard;
    string playerFourPowerUpController;
    KeyCode playerFourPowerUpKeyboard;

    float player1x;
    float player1z;

    void Start()
    {
        distanceToGround = transform.position.y;
        tempMaxSpeed = maxSpeed;
        tempDashPower = 1;

        //Player One
        playerOneJumpController = "joystick 1 button 4"; //Left bumper controller 1
        playerOneJumpKeyboard = KeyCode.Space;
        playerOneDashController = "joystick 1 button 8"; //Left stick pushed down controller 1
        playerOneDashKeyboard = KeyCode.Q;
        playerOnePowerUpController = "joystick 1 button 2";
        playerOnePowerUpKeyboard = KeyCode.Z;

        //Player Two
        playerTwoJumpController = "joystick 1 button 5"; //Right bumper controller 1
        playerTwoJumpKeyboard = KeyCode.RightControl;
        playerTwoDashController = "joystick 1 button 9"; //Right stick pushed down controller 1
        playerTwoDashKeyboard = KeyCode.E;
        playerTwoPowerUpController = playerOnePowerUpController;
        playerTwoPowerUpKeyboard = KeyCode.P;

        //Player Three
        playerThreeJumpController = "joystick 2 button 4"; //Left bumper controller 2
        playerThreeJumpKeyboard = KeyCode.Space;
        playerThreeDashController = "joystick 2 button 8"; //Left stick pushed down controller 2
        playerThreeDashKeyboard = KeyCode.Period;
        playerThreePowerUpController = "joystick 2 button 2";
        playerThreePowerUpKeyboard = KeyCode.Comma;

        //Player Four
        playerFourJumpController = "joystick 2 button 5"; //Right bumper controller 2
        playerFourJumpKeyboard = KeyCode.Space;
        playerFourDashController = "joystick 2 button 9"; //Right stick pushed down controller 2
        playerFourDashKeyboard = KeyCode.Slash;
        playerFourPowerUpController = playerThreePowerUpController;
        playerFourPowerUpKeyboard = KeyCode.RightShift;
    }

    void Update()
    {
        if(GameManager.singleton.matchStarted)
        {
            player1x = Input.GetAxis(horizontalAxis) * Time.deltaTime * acceleration * (1- friction);
            player1z = Input.GetAxis(verticalAxis) * Time.deltaTime * acceleration * (1 - friction);

            //Actions
            if (transform.parent.name == "Team 0")
            {
                //Player 1
                if ((Input.GetKeyDown(playerOneJumpController) || Input.GetKeyDown(playerOneJumpKeyboard)) && isGrounded() && team == 1) { Jump(); }
                if (Input.GetKeyDown(playerOneDashController) || Input.GetKeyDown(playerOneDashKeyboard) && team == 1) { Dash(); }

                //Player 2
                if ((Input.GetKeyDown(playerTwoJumpController) || Input.GetKeyDown(playerTwoJumpKeyboard)) && isGrounded() && team == 2) { Jump(); }
                if (Input.GetKeyDown(playerTwoDashController) || Input.GetKeyDown(playerTwoDashKeyboard) && team == 2) { Dash(); }

                // Active powerUp if Team has one
                if (Input.GetKeyDown(playerOnePowerUpKeyboard) || Input.GetKeyDown(playerTwoPowerUpKeyboard) ||
                    Input.GetKeyDown(playerOnePowerUpController) || Input.GetKeyDown(playerTwoPowerUpController))
                {
                    GameObject powerUp = gameObject.GetComponentInParent<Team>().CurrentPowerUp;
                    if (powerUp != null && powerUp.GetComponent<PowerUp>().isActive != true)
                    {
                        powerUp.GetComponent<PowerUp>().Activate();
                    }
                }
            }
            if (transform.parent.name == "Team 1")
            {
                //Player 3
                if ((Input.GetKeyDown(playerThreeJumpController) || Input.GetKeyDown(playerThreeJumpKeyboard)) && isGrounded() && team == 1) { Jump(); }
                if (Input.GetKeyDown(playerThreeDashController) || Input.GetKeyDown(playerThreeDashKeyboard) && team == 1) { Dash(); }

                //Player 4
                if ((Input.GetKeyDown(playerFourJumpController) || Input.GetKeyDown(playerFourJumpKeyboard)) && isGrounded() && team == 2) { Jump(); }
                if (Input.GetKeyDown(playerFourDashController) || Input.GetKeyDown(playerFourDashKeyboard) && team == 2) { Dash(); }

                // Activate powerUp if Team has one
                if (Input.GetKeyDown(playerThreePowerUpKeyboard) || Input.GetKeyDown(playerFourPowerUpKeyboard) ||
                    Input.GetKeyDown(playerThreePowerUpController) || Input.GetKeyDown(playerFourPowerUpController))
                {
                    GameObject powerUp = gameObject.GetComponentInParent<Team>().CurrentPowerUp;
                    if (powerUp != null && powerUp.GetComponent<PowerUp>().isActive != true)
                    {
                        powerUp.GetComponent<PowerUp>().Activate();
                    }
                }
            }

            if (dashing)
            {
                timeSinceDash += Time.deltaTime;
            }
            if (timeSinceDash >= dashTime)
            {
                UnDash();
            }

            //Debug.Log(transform.parent.name + " " + team + ": " + tempDashPower + ", " + tempMaxSpeed + " | " + dashing + ", " + timeSinceDash.ToString("0.00"));
            GetComponent<Rigidbody>().AddForce(player1x * tempDashPower, 0, player1z * tempDashPower, ForceMode.Acceleration);
            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, tempMaxSpeed);

            //Slow down
            GetComponent<Rigidbody>().velocity *= (1 - decelerationRate);
        }
    }

    public void SetControls(string vertical, string horizontal)
    {
        verticalAxis = vertical;
        horizontalAxis = horizontal;
    }

    public void SetTeam(int num)
    {
        team = num;
    }

    public void SetJumpPower(float n)
    {
        jumpPower = n;
    }

    public void SetDashPower(float n)
    {
        dashPower = n;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    void Jump()
    {
        GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
    }

    void Dash()
    {
        if (dashing)
        {
            return;
        }

        tempDashPower = dashPower;
        tempMaxSpeed = maxSpeed * 2;
        dashing = true;
    }

    void UnDash()
    {
        tempDashPower = 1;
        tempMaxSpeed = maxSpeed;
        dashing = false;
        timeSinceDash = 0;
    }
}