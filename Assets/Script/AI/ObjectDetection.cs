using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* @author: Diego Wilde
 *   @date: October 2017
 *  @title: ObjectDetection.cs
 *  
 *  This script will contain some methods that can be used for the AI to find other objects
 *  in the game, or info about those objects such as their distance from the AI player team.
 **/

public class ObjectDetection : MonoBehaviour {

    public float maxDetectionRange = 15.0f;

	// Update is called once per frame
	void Update () {
        // Ideally, we want to use a point actually on the rope ( see below )
        Vector3 midAIpoint = getAverageAIPlayerPos();
        float nearestDist = distanceToNearestObjectfromPoint(midAIpoint, "Player");
        if (nearestDist <= maxDetectionRange){
            Debug.Log("player found " + nearestDist + " from AI midpoint");
        }
    }
    
    // This method finds the shortest distance between the given Vector3 and all objects with the given tag
    public float distanceToNearestObjectfromPoint(Vector3 point, string tag){
        float distance = Mathf.Infinity;
        float tempDist = Mathf.Infinity;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach( GameObject thing in objects){
            float thingX = thing.transform.position.x;
            float thingY = thing.transform.position.y;
            float thingZ = thing.transform.position.z;
            tempDist = Mathf.Sqrt(Mathf.Pow(point.x - thingX, 2) + Mathf.Pow(point.y - thingY, 2) + Mathf.Pow(point.z - thingZ, 2));
            if (tempDist < distance)
                distance = tempDist;
        }
        return distance;
    }

    // This is pretty much the same method from CameraControl
    private Vector3 getAverageAIPlayerPos(){
        Vector3 midPlayerPoint;
        float aveX, aveY, aveZ;
        aveX = 0.0f;
        aveY = 0.0f;
        aveZ = 0.0f;
        GameObject[] aiplayers = GameObject.FindGameObjectsWithTag("AIPlayer");
        foreach (GameObject aiplayer in aiplayers){
            aveX += aiplayer.transform.position.x;
            aveY += aiplayer.transform.position.y;
            aveZ += aiplayer.transform.position.z;
        }
        aveX = (float)aveX / aiplayers.Length;
        aveY = (float)aveY / aiplayers.Length;
        aveZ = (float)aveZ / aiplayers.Length;
        midPlayerPoint = new Vector3(aveX, aveY, aveZ);
        return midPlayerPoint;
    }

    // This method should get the point between two players on the rope
    // Currently broken
    public Vector3 midRopePos(Team team){
        // the below should work if we make the catch points public (???)

        //if (!team.currentRope)
        //    return Vector3.negativeInfinity;
        //float dist = 0.0f;
        //float midDist = 0.0f;
        //for (int i = 1; i < Team.currentRope.visiblePoints.length; ++i)
        //{
        //    dist += Vector3.Distance(visiblePoints[i], visiblePoints[i - 1]);
        //}
        //midDist = dist / 2.0f;
        // // make rays that follow the rope, use ray.getPoint() to find 
        // // the midpoint, which is what we want


        return Vector3.zero;
    }




}
