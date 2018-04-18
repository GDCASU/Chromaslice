using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* @title: CameraControl.cs
 * @author: Diego Wilde
 * @date: September 18th, 2017
 * @description: This script should function to keep all players within
 *               the main view at all times during the game.
 */

// Kyle Aycock 11/17/17 - fixed an error having to do with unhandled zero-player case
 
// Diego Wilde 3/27/18 improved camera smoothing
public class CameraControl : MonoBehaviour
{

    private Camera cam;
    private Transform camPos;
    private Vector3 camPosVector;
    private Vector3 lastPosition;
    private float lastNearestZPos;
    private GameObject[] players;
    //Degree of leniency before the camera pans
    public float deltaX = 3.0f;
    public float deltaZ = 0.0f;
    public float yPosition = 0.0f;
    public bool gamePaused;
    //restricts camera movement to one dimension
    public bool lockAxis = true;

    private void Awake () {
        cam = GetComponent<Camera>();
        camPos = this.transform;
        players = null;
    }

    //DW: Each frame changes the camera focus and position
    public void Update ()
    {
        GetPlayers();

        if (players.Length > 0)
        {

            camPosVector = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z);

            // Pan the camera
            Vector3 moveTo = Vector3.zero;
            if (camPos.position.x < getAveragePlayerPos().x - deltaX)
                moveTo = new Vector3((getAveragePlayerPos().x - deltaX) - camPos.position.x, 0.0f, 0.0f);
            if (camPos.position.x > getAveragePlayerPos().x + deltaX)
                moveTo = new Vector3((getAveragePlayerPos().x + deltaX) - camPos.position.x, 0.0f, 0.0f);
            //This code should work the same for forward / back panning, but it's pretty wack right now

            if (lockAxis) // pins the camera to movement only in the x direction
                camPos.Translate(moveTo, Space.World);
            else
                camPos.Translate(moveTo, camPos);
            // this kind of panning is a little choppy; I think its something to do with transform.Translate

            float newZ = getNearestPlayerZPos() - deltaZ;
            this.transform.position = new Vector3(this.transform.position.x, yPosition, newZ);

            //Set the camera to the optimal focal point
            //camPos.LookAt(getAveragePlayerPos(), Vector3.up); //old

            //DW: interpolate each rotation between the last focus and the new one; should provide 
            //    smoother transitions when players are removed / added
            Quaternion newRotation = Quaternion.LookRotation(getAveragePlayerPos() - camPos.position);
            camPos.rotation = Quaternion.Slerp(camPos.rotation, newRotation, Time.deltaTime);


            lastPosition = getAveragePlayerPos();
            lastNearestZPos = getNearestPlayerZPos();
        }
    }

    //DW: This method gives us the midpoint between all players
    private Vector3 getAveragePlayerPos(){
        Vector3 midPlayerPoint;
        float aveX, aveY, aveZ;
        aveX = 0.0f;
        aveY = 0.0f;
        aveZ = 0.0f;
        int activePlayers = 0;
        foreach(GameObject player in players)
        {
            if (player != null && player.activeSelf)
            {
                activePlayers++;
                aveX += player.transform.position.x;
                aveY += player.transform.position.y;
                aveZ += player.transform.position.z;
            }
        }

        if (activePlayers <= 0)
            return lastPosition;
        else
        {
            aveX = aveX / activePlayers;
            aveY = aveY / activePlayers;
            aveZ = aveZ / activePlayers;
            midPlayerPoint = new Vector3(aveX, aveY, aveZ);
            return midPlayerPoint;
        }
    }

    // get the Z of the player closest to z = negative infinite
    private float getNearestPlayerZPos()
    {
        float farPoint = Mathf.Infinity;
        if (players.Length == 0)
            return 0;

        int activePlayers = 0;
        foreach (GameObject player in players)
        {
            if (player != null && player.activeSelf && player.transform.position.z < farPoint)
            {
                activePlayers++;
                farPoint = player.transform.position.z;
            }
        }

        if (activePlayers <= 0)
            return lastNearestZPos;
        else
            return farPoint;
    }

    private void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}