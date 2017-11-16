using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* @author: Diego Wilde
 *  @title: AILibrary.cs
 *   @date: November 2017
 * 
 * This script is a library of functions to be used for whatever purpose you need them for.
 **/

public static class AILibrary{
    #region find

    // This method returns the floating point distance from a center point to 
    // the location of the nearest object with the given tag.
    public static float distanceToNearestObjectfromPoint(Vector3 point, string tag)
    {
        float distance = Mathf.Infinity;
        float tempDist = Mathf.Infinity;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject thing in objects)
        {
            float thingX = thing.transform.position.x;
            float thingY = thing.transform.position.y;
            float thingZ = thing.transform.position.z;
            tempDist = Mathf.Sqrt(Mathf.Pow(point.x - thingX, 2) + Mathf.Pow(point.y - thingY, 2) + Mathf.Pow(point.z - thingZ, 2));
            if (tempDist < distance)
                distance = tempDist;
        }
        return distance;
    }

    // Gets the GameObject of the nearest object with the given tag to the
    // center point.
    public static GameObject NearestObjectfromPoint(Vector3 point, string tag)
    {
        float distance = Mathf.Infinity;
        float tempDist = Mathf.Infinity;
        GameObject nearestOb = null;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject thing in objects)
        {
            float thingX = thing.transform.position.x;
            float thingY = thing.transform.position.y;
            float thingZ = thing.transform.position.z;
            tempDist = Mathf.Sqrt(Mathf.Pow(point.x - thingX, 2) + Mathf.Pow(point.y - thingY, 2) + Mathf.Pow(point.z - thingZ, 2));
            if (tempDist < distance)
            {
                distance = tempDist;
                nearestOb = thing;
            }
        }
        return nearestOb;
    }
    #endregion

    #region locations

    // Returns the Vector3 Midpoint between players tagged as AI
   public static Vector3 getAverageAIPlayerPos(){
        Vector3 midPlayerPoint;
        float aveX, aveY, aveZ;
        aveX = 0.0f;
        aveY = 0.0f;
        aveZ = 0.0f;
        GameObject[] aiplayers = GameObject.FindGameObjectsWithTag("AIPlayer");
        foreach (GameObject aiplayer in aiplayers)
        {
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

    // Gives us the Vector3 Midpoint between ALL players in the level.
    public static Vector3 getAveragePlayerPos(){
        Vector3 midPlayerPoint;
        float aveX, aveY, aveZ;
        aveX = 0.0f;
        aveY = 0.0f;
        aveZ = 0.0f;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            aveX += player.transform.position.x;
            aveY += player.transform.position.y;
            aveZ += player.transform.position.z;
        }
        aveX = (float)aveX / players.Length;
        aveY = (float)aveY / players.Length;
        aveZ = (float)aveZ / players.Length;
        midPlayerPoint = new Vector3(aveX, aveY, aveZ);
        return midPlayerPoint;
    }

    #endregion
}
