using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

/*
*Developer:      Ryan Black
*Date:           10/18/2017
*Description:    Changed Dash to Sprint and created new Dash functions
*/

// Developer:   Kyle Aycock
// Date:        10/27/2017
// Description: Reworked movement from acceleration to velocity based
//              Made tops stick to ground regardless of slopes and such
//              Added "Beyblade Effect" - tops bounce off each other
//              violently when colliding

public class PlayerController : NetworkBehaviour
{

    //Game attributes
    public float numPlayers = 1; //is this even needed anymore?

    //Player attributes
    public float health = 100;
    public float maxSpeed;
    public float bounceFactor;
    public float sprintPower = 5;
    public float sprintTime = 2;
    public float decelerationRate = 0.2f;
    public float dashPower = 5;
    public float dashTime = 1;
    public float dashCooldown = 3;
    public int team;

    private float tempMovementPower;
    private float tempMaxSpeed;

    private bool sprinting = false;
    private bool dashing = false;
    private Vector3 targetLock;
    private float timeSinceSprint = 0;
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

    float distanceToGround; //to prevent jumping in the air
    public Controls controls;

    void Start()
    {
        distanceToGround = GetComponent<SphereCollider>().radius;
        tempMaxSpeed = maxSpeed;
        tempMovementPower = 1;
        targetLock = Vector3.zero;
    }

    void LateUpdate()
    {
        //Sticks player to terrain
        RaycastHit ground = new RaycastHit();
        if (Physics.SphereCast(new Ray(transform.position + Vector3.up, Vector3.down), distanceToGround, out ground, 10f, LayerMask.GetMask("Terrain")))
            transform.position = new Vector3(transform.position.x, transform.position.y - ground.distance + 1, transform.position.z);
    }

    void FixedUpdate()
    {
        if (GameManager.singleton.matchStarted && isLocalPlayer)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (controls.GetDash() && !dashing && timeSinceDash >= dashCooldown) { Dash(); timeSinceDash = 0; }

            // Active powerUp if Team has one
            if (controls.GetPowerUp())
            {
                GameObject powerUp = gameObject.GetComponentInParent<Team>().CurrentPowerUp;
                if (powerUp != null && powerUp.GetComponent<PowerUp>().isActive != true)
                {
                    powerUp.GetComponent<PowerUp>().Activate();
                }
            }

            if (sprinting)
            {
                timeSinceSprint += Time.deltaTime;
            }
            if (timeSinceSprint >= sprintTime)
            {
                UnSprint();
            }

            timeSinceDash += Time.deltaTime;

            if (timeSinceDash >= dashTime)
            {
                UnDash();
            }


            Vector3 target = new Vector3(controls.GetHorizontal(), 0, controls.GetVertical()).normalized * maxSpeed;
            Vector3 accel;
            if (dashing)
                accel = targetLock - rb.velocity;
            else
                accel = new Vector3((target.x - rb.velocity.x) * decelerationRate, 0, (target.z - rb.velocity.z) * decelerationRate);
            accel.y = 0;

            rb.AddForce(accel, ForceMode.VelocityChange);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Beyblade effect - sends both tops flying in the direction of the normal they collided with based on bounceFactor
        if (collision.gameObject.tag == "Player")
            GetComponent<Rigidbody>().AddForce((collision.relativeVelocity * bounceFactor).magnitude * collision.contacts[0].normal, ForceMode.VelocityChange);
    }

    public void SetControls(int controller)
    {
        controls = Controls.LoadFromConfig(controller);
    }

    public void SetTeam(int num)
    {
        team = num;
    }


    public void SetDashPower(float n)
    {
        sprintPower = n;
    }

    void Dash()
    {
        if (dashing)
        {
            return;
        }
        Debug.Log(dashing);

        dashing = true;
        timeSinceDash = 0;
        targetLock = new Vector3(controls.GetHorizontal(), 0, controls.GetVertical()).normalized * maxSpeed * dashPower;
    }

    void UnDash()
    {
        tempMovementPower = 1;
        tempMaxSpeed = maxSpeed;
        dashing = false;
    }

    void Sprint()
    {
        if (sprinting)
        {
            return;
        }

        tempMovementPower = sprintPower;
        tempMaxSpeed = maxSpeed * 2;
        sprinting = true;
    }

    void UnSprint()
    {
        tempMovementPower = 1;
        tempMaxSpeed = maxSpeed;
        sprinting = false;
        timeSinceSprint = 0;
    }
}