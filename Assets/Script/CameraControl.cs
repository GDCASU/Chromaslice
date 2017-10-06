using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* @title: CameraControl.cs
 * @author: Diego Wilde
 * @date: September 18th, 2017
 * @description: This script should function to keep all players within
 *               the main view at all times during the game.
 */
public class CameraControl : MonoBehaviour {

    private Camera cam;
    private Transform camPos;
    private Vector3 camPosVector;
    public float deltaX = 3.0f;
    public float deltaZ = 50.0f;
    public bool gamePaused;
    //restricts camera movement to one dimension
    public bool lockAxis = true;

    private void Awake () {
        cam = GetComponent<Camera>();
        camPos = this.transform;
    }


    //DW: Each frame changes the camera focus and position
    void Update () {
        camPosVector = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z);
        
        //Set the camera to the optimal focal point
        camPos.LookAt(getAveragePlayerPos(), Vector3.up);

        // Pan the camera
        Vector3 moveTo = Vector3.zero;
        if (camPos.position.x < getAveragePlayerPos().x - deltaX)
                moveTo = new Vector3((getAveragePlayerPos().x - deltaX) - camPos.position.x, 0.0f, 0.0f);
        if (camPos.position.x > getAveragePlayerPos().x + deltaX)
                moveTo = new Vector3((getAveragePlayerPos().x + deltaX) - camPos.position.x, 0.0f, 0.0f);
         //This code should work the same for forward / back panning, but it's pretty wack right now
         /**
        if (camPos.position.z < getAveragePlayerPos().z - deltaZ)
            moveTo = new Vector3(0.0f, 0.0f, (getAveragePlayerPos().z - deltaZ) - camPos.position.z);
        if (camPos.position.z > getAveragePlayerPos().z)
            moveTo = new Vector3(0.0f, 0.0f, (getAveragePlayerPos().z) - camPos.position.z);
        **/

        if (lockAxis) // pins the camera to movement only in the x direction
            camPos.Translate(moveTo, Space.World);
        else
            camPos.Translate(moveTo, camPos);
        // this kind of panning is a little choppy; I think its something to do with transform.Translate

    }

    //DW: This method gives us the midpoint between all players
    private Vector3 getAveragePlayerPos(){
        Vector3 midPlayerPoint;
        float aveX, aveY, aveZ;
        aveX = 0.0f;
        aveY = 0.0f;
        aveZ = 0.0f;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            aveX += player.transform.position.x;
            aveY += player.transform.position.y;
            aveZ += player.transform.position.z;
        }
        aveX = (float) aveX / players.Length;
        aveY = (float) aveY / players.Length;
        aveZ = (float) aveZ / players.Length;
        midPlayerPoint = new Vector3(aveX, aveY, aveZ);
        return midPlayerPoint;
    }



}
