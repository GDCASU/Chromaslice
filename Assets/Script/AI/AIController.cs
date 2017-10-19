using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class serves as a hub for common AI functions to be used within states, such as MoveToPoint.
 *                  Currently, MoveToPoint Leprs bewteen two distances, but this will most likely need to change 
 *                  if we want the AI to move in the same way that players do.
 * Author:          Zachary Schmalz
 * Date:            10/18/2017
 */

public class AIController : MonoBehaviour
{
    private Team team;
    private Vector3 start;
    private Vector3 end;

    private float speed = 5.0f;
    private float startTime;
    private float distance;

    private bool isMovingToPoint;

	void Start ()
    {
        team = gameObject.GetComponentInParent<Team>();
        start = gameObject.transform.position;
        isMovingToPoint = false;
	}
	
	void Update ()
    {
        // MoveToPoint
        if(isMovingToPoint)
        {
            float distanceMoved = (Time.time - startTime) * speed;
            float journey = distanceMoved / distance;
            transform.position = Vector3.Lerp(start, end, journey);

            if (transform.position == end)
                isMovingToPoint = false;
        }
    }

    // Function to move to a specified point
    public void MoveToPoint(Vector3 point)
    {
        startTime = Time.time;
        end = point;
        distance = Vector3.Distance(start, end);
        isMovingToPoint = true;
    }
}