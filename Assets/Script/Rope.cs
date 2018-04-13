using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Developer:   Kyle Aycock
// Date:        8/28/17
// Description: This class represents the rope connection between two end points.

/*
* Version 1.1.0
* Author: Zachary Schmalz
* Date: September 27, 2017
* Revisions: Added checks for collisions with PowerUps and Projectiles.
*/

// Developer:   Ryan Black
// Date:        10/18/17
// Description: Added Rope cooldown on breaking and some rope reform rules

// Developer:   Kyle Aycock
// Date:        10/27/17
// Description: Added rope relaxation for vertically oriented catch points too

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Networked the endpoints so the rope works properly on clients

public class Rope : NetworkBehaviour
{
    //rope parameters
    public float maxRopeLength;
    public float maxRopeWidth;
    public float minRopeWidth;
    public Color ropeStretchColor;
    public Color ropeLaxColor;

    //transforms that each end of rope is anchored to
    [SyncVar(hook = "SetEndPoint1")]
    private NetworkInstanceId endPoint1Id;
    [SyncVar(hook = "SetEndPoint2")]
    private NetworkInstanceId endPoint2Id;

    private Transform endPoint1;
    private Transform endPoint2;

    //keeps track of last position in order to ignore frames on which nothing changed
    private Vector3 oldPos1;
    private Vector3 oldPos2;

    //list of world positions at which the rope bends
    private List<Vector3> ropePoints;
    private Vector3[] visiblePoints; //array form including endpoints

    //list of booleans indicating whether that bend is CW/CCW
    private List<bool> anglesPositive;
    private List<Vector3> ropeNormals;

    //renderer for the rope
    private LineRenderer line;

    // Fields needed for collecting PowerUps
    //public GameObject powerUp;
    private int rotations;
    private bool collectedPowerUp;

    // Fields needed for interacting with projectiles
    private GameObject projectile;
    private bool hitProjectile;

    // The Team script that is attached to the parent
    private Team team;

    //Rope Reforming variables
    public float reformCooldown = 3;
    private float timeSinceBreaking = 0;

    [SyncVar]
    public NetworkInstanceId parentId;

    // Use this for initialization
    void Start()
    {
        ropePoints = new List<Vector3>();
        anglesPositive = new List<bool>();
        ropeNormals = new List<Vector3>();
        line = GetComponent<LineRenderer>();
        visiblePoints = new Vector3[2];
        if (endPoint1)
        {
            visiblePoints[0] = endPoint1.transform.position;
            oldPos1 = visiblePoints[0];
        }
        if (endPoint2)
        {
            visiblePoints[1] = endPoint2.transform.position;
            oldPos2 = visiblePoints[1];
        }
        gameObject.layer = transform.parent.gameObject.layer;
        rotations = 0;
        collectedPowerUp = false;
        //powerUp = null;
        projectile = null;
        hitProjectile = false;
        team = gameObject.GetComponentInParent<Team>();
    }

    public override void OnStartClient()
    {
        if (!endPoint1Id.IsEmpty()) SetEndPoint1(endPoint1Id);
        if (!endPoint2Id.IsEmpty()) SetEndPoint2(endPoint2Id);
        if (!parentId.IsEmpty()) transform.SetParent(ClientScene.FindLocalObject(parentId).transform);
    }

    [Server]
    public void SetEndpoints(Transform first, Transform second)
    {
        endPoint1 = first;
        endPoint2 = second;
        endPoint1Id = first.GetComponent<NetworkIdentity>().netId;
        endPoint2Id = second.GetComponent<NetworkIdentity>().netId;
    }

    public void SetEndPoint1(NetworkInstanceId id)
    {
        endPoint1Id = id;
        endPoint1 = ClientScene.FindLocalObject(id).transform;
        oldPos1 = endPoint1.transform.position;
    }

    public void SetEndPoint2(NetworkInstanceId id)
    {
        endPoint2Id = id;
        endPoint2 = ClientScene.FindLocalObject(id).transform;
        oldPos2 = endPoint2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisiblePoints();
        float dist = 0;

        for (int i = 1; i < visiblePoints.Length; i++)
            dist += Vector3.Distance(visiblePoints[i], visiblePoints[i - 1]);
        UpdateRopeDisplay(dist);
        if (dist > maxRopeLength)
        {
            BreakRope();
        }
    }

    //Updates the actual line renderer with the current rope information
    public void UpdateRopeDisplay(float dist)
    {
        line.positionCount = visiblePoints.Length;
        line.SetPositions(visiblePoints);
        line.startWidth = Mathf.Lerp(maxRopeWidth, minRopeWidth, dist / maxRopeLength);
        line.endWidth = line.startWidth;
        line.startColor = Color.Lerp(ropeLaxColor, ropeStretchColor, dist / maxRopeLength);
        line.endColor = line.startColor;
    }

    //Makes the visiblePoints array from end point and bend point information
    public void UpdateVisiblePoints()
    {
        visiblePoints = new Vector3[ropePoints.Count + 2];
        visiblePoints[0] = endPoint1.position;
        for (int i = 1; i < visiblePoints.Length - 1; i++)
            visiblePoints[i] = ropePoints[i - 1];
        visiblePoints[visiblePoints.Length - 1] = endPoint2.position;
    }

    //Destroys the rope
    public void BreakRope()
    {
        Debug.Log("Rope was destroyed.");

        // If the rope was in a state of launching a projectile when it breaks, call the projectile code. Pass the vector containing all rope points
        // Also set the Team that launches the projectile
        if (hitProjectile && projectile != null)
        {
            projectile.GetComponent<Projectile>().Team = team;
            projectile.GetComponent<Projectile>().LaunchProjectile(ropePoints);
        }

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        //"physical" elements - rope catching on things, necessary raycasts
        //must check both directions

        //visiblePoints[] serves as an unchanging record of what the rope was like at the start of this physics frame

        UpdateVisiblePoints();

        Vector3 start = endPoint1.transform.position;
        Vector3 end = endPoint2.transform.position;

        RaycastHit hit;

        if (isServer)
        {
            for (int i = 0; i < visiblePoints.Length - 1; i++)
                if (RaycastSegment(visiblePoints[i], visiblePoints[i + 1], out hit, gameObject.layer))
                    if (hit.collider.GetComponent<PlayerController>())
                        hit.collider.transform.parent.GetComponent<Team>().KillTeam(hit.collider.gameObject);
        }


        //work in progress
        /*for (int i = 0; i < ropePoints.Count; i++)
        {
            if (Physics.CheckSphere(ropePoints[i], 0.02f))
                ropePoints[i] += Vector3.Project((Vector3.Project(ropePoints[i] - visiblePoints[i], (visiblePoints[i + 2] - visiblePoints[i]).normalized) + visiblePoints[i] - ropePoints[i]), ropeNormals[i]);
            else
                ropePoints[i] += (Vector3.Project(ropePoints[i] - visiblePoints[i], (visiblePoints[i + 2] - visiblePoints[i]).normalized) + visiblePoints[i] - ropePoints[i]);
        }*/


        if (start != oldPos1 || end != oldPos2)
        {
            if (start != oldPos1)
            {
                float segments = Mathf.Ceil((start - oldPos1).magnitude * 4);
                for (int i = 0; i < segments; i++)
                {
                    if (RaycastSegment(Vector3.Lerp(oldPos1, start, i / segments), visiblePoints[1], out hit, gameObject.layer))
                    {
                        Vector3 newPoint = hit.point;
                        newPoint += (hit.point - hit.collider.transform.position).normalized * 0.01f;
                        ropePoints.Insert(0, newPoint);
                        Vector3 n = Vector3.Cross(newPoint - start, visiblePoints[1] - start).normalized;
                        ropeNormals.Insert(0, n);
                        anglesPositive.Insert(0, IsAnglePositive(start, newPoint, visiblePoints[1], n));
                        //Debug.Log("Adding new catch point at index 0" + " (1)");

                        //// If the hit point object is a PowerUp
                        //if (hit.collider.gameObject.GetComponent<PowerUp>())
                        //{
                        //    powerUp = hit.collider.gameObject;
                        //    //CollisionWithPowerUp();
                        //}

                        // If the hit point object is a Projectile
                        if (hit.collider.gameObject.GetComponent<Projectile>())
                        {
                            projectile = hit.collider.gameObject;
                            hitProjectile = true;
                        }
                        else
                        {
                            projectile = null;
                            hitProjectile = false;
                        }

                        return;
                    }
                }
            }
            if (end != oldPos2)
            {
                float segments = Mathf.Ceil((end - oldPos2).magnitude * 4);
                for (int i = 0; i < segments; i++)
                {
                    if (RaycastSegment(Vector3.Lerp(oldPos2, end, i / segments), visiblePoints[visiblePoints.Length - 2], out hit, gameObject.layer))
                    {
                        Vector3 newPoint = hit.point;
                        newPoint += (hit.point - hit.collider.transform.position).normalized * 0.01f;
                        ropePoints.Add(newPoint);
                        Vector3 n = Vector3.Cross(newPoint - end, visiblePoints[visiblePoints.Length - 2] - end).normalized;
                        ropeNormals.Add(n);
                        anglesPositive.Add(IsAnglePositive(visiblePoints[visiblePoints.Length - 2], newPoint, end, n));
                        //Debug.Log("Adding new catch point at index " + (ropePoints.Count - 1) + " (2)");
                        return;
                    }
                }
            }

            //check whether to relax rope
            if (visiblePoints.Length > 2)
            {
                int len = visiblePoints.Length;

                if (IsAnglePositive(start, visiblePoints[1], visiblePoints[2], ropeNormals[0]) != anglesPositive[0])
                {
                    ropePoints.RemoveAt(0);
                    anglesPositive.RemoveAt(0);
                    ropeNormals.RemoveAt(0);
                    //Debug.Log("Relaxing rope at 0" + " (1)");
                }
                else if (IsAnglePositive(visiblePoints[len - 3], visiblePoints[len - 2], end, ropeNormals[ropeNormals.Count - 1]) != anglesPositive[ropePoints.Count - 1])
                {
                    ropePoints.RemoveAt(ropePoints.Count - 1);
                    anglesPositive.RemoveAt(anglesPositive.Count - 1);
                    ropeNormals.RemoveAt(ropeNormals.Count - 1);
                    //Debug.Log("Relaxing rope at " + ropePoints.Count + " (2)");
                }
            }

            oldPos1 = start;
            oldPos2 = end;
        }
        
    }

    //// Function that determines how many times the rope is wrapped around a PowerUp
    //private void CollisionWithPowerUp()
    //{
    //    if (visiblePoints.Length > 3)
    //    {
    //        float totalDist = 0.0f;
    //        // Calculate the total distance between all the visiblePoints on the rope
    //        for (int i = 1; i < visiblePoints.Length - 1; i++)
    //        {
    //            if (i + 1 != visiblePoints.Length - 1)
    //                totalDist += Vector3.Distance(visiblePoints[i], visiblePoints[i + 1]);
    //        }
    //        // Get the radius of the PowerUp object. For this to work, PowerUps must have a SphereCollider
    //        float radius = powerUp.GetComponent<SphereCollider>().radius * powerUp.transform.localScale.y;

    //        // Math MAGIC. Calcultes the number of rotations by dividing the total distance of all the rope points by the distance(circumference) of the SphereObject
    //        // where the rope is colliding with the Sphere.
    //        rotations = (int)(totalDist / (Math.PI * (2 * (Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(radius - Math.Abs(powerUp.transform.position.y - radius - .5), 2))))));

    //        // Need 1 rotation to collect the PowerUp
    //        if (rotations > 0)
    //            collectedPowerUp = true;
    //    }
    //}

    private bool IsAnglePositive(Vector3 a, Vector3 b, Vector3 c, Vector3 n)
    {
        Vector3 angle1 = (a - b).normalized;
        Vector3 angle2 = (c - b).normalized;
        return Mathf.Atan2(Vector3.Dot(Vector3.Cross(angle2, angle1), n), Vector3.Dot(angle1, angle2)) > 0; //voodoo magic
    }

    public static bool RaycastSegment(Vector3 from, Vector3 to, out RaycastHit hit, int ignoreLayer = -1)
    {
        Vector3 diff = to - from;
        return Physics.Raycast(from, diff.normalized, out hit, diff.magnitude, ~(1 << ignoreLayer)); //ignore player layer
    }

    public static bool RaycastSegment(Vector3 from, Vector3 to, int ignoreLayer = -1)
    {
        RaycastHit hit;
        return RaycastSegment(from, to, out hit, ignoreLayer);
    }
}
