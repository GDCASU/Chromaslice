using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        8/28/17
// Description: This class represents the rope connection between two end points.
//              

public class Rope : MonoBehaviour
{
    //rope parameters
    public float maxRopeLength;
    public float maxRopeWidth;
    public float minRopeWidth;
    public Color ropeStretchColor;
    public Color ropeLaxColor;

    //transforms that each end of rope is anchored to
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

    //renderer for the rope
    private LineRenderer line;


    // Use this for initialization
    void Start()
    {
        ropePoints = new List<Vector3>();
        anglesPositive = new List<bool>();
        line = GetComponent<LineRenderer>();
        visiblePoints = new Vector3[2];
        visiblePoints[0] = endPoint1.position;
        visiblePoints[1] = endPoint2.position;

        gameObject.layer = transform.parent.gameObject.layer;
    }

    public void SetEndpoints(Transform first, Transform second)
    {
        endPoint1 = first;
        endPoint2 = second;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisiblePoints();
        float dist = 0;
        for (int i = 1; i < visiblePoints.Length; i++)
            dist += Vector3.Distance(visiblePoints[i], visiblePoints[i - 1]);
        if (dist > maxRopeLength)
            BreakRope();
        else
            UpdateRopeDisplay(dist);
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

        RaycastHit[] hits = new RaycastHit[visiblePoints.Length - 1];
        bool[] results = new bool[hits.Length]; //true if hit terrain, false if otherwise (player/nothing)

        for (int i = 0; i < visiblePoints.Length - 1; i++)
        {
            if (RaycastSegment(visiblePoints[i], visiblePoints[i + 1], out hits[i], gameObject.layer))
            {
                if (hits[i].collider.GetComponent<PlayerController>())
                    hits[i].collider.transform.parent.GetComponent<Team>().KillTeam();
                else
                    results[i] = true;
            }
        }

        if (start != oldPos1 || end != oldPos2)
        {

            RaycastHit hit;

            if (results[0])
            {
                hit = hits[0];
                Vector3 newPoint = hit.point;
                newPoint += (hit.point - hit.collider.transform.position).normalized * 0.01f;
                ropePoints.Insert(0, newPoint);
                anglesPositive.Insert(0, IsAnglePositive(start, newPoint, visiblePoints[1]));
                //Debug.Log("Adding new catch point at index 0" + " (1)");
                return;
            }
            //it's better to have this one pointing backwards, requires extra raycast but oh well
            if (RaycastSegment(end, visiblePoints[visiblePoints.Length - 2], out hit, gameObject.layer))
            {
                Vector3 newPoint = hit.point;
                newPoint += (hit.point - hit.collider.transform.position).normalized * 0.01f;
                ropePoints.Add(newPoint);
                anglesPositive.Add(IsAnglePositive(visiblePoints[visiblePoints.Length - 2], newPoint, end));
                //Debug.Log("Adding new catch point at index " + (ropePoints.Count - 1) + " (2)");
                return;
            }

            //check whether to relax rope
            if (visiblePoints.Length > 2)
            {
                int len = visiblePoints.Length;
                if (IsAnglePositive(start, visiblePoints[1], visiblePoints[2]) != anglesPositive[0])
                {
                    ropePoints.RemoveAt(0);
                    anglesPositive.RemoveAt(0); 
                    //Debug.Log("Relaxing rope at 0" + " (1)");
                }
                else if (IsAnglePositive(visiblePoints[len - 3], visiblePoints[len - 2], end) != anglesPositive[ropePoints.Count - 1])
                {
                    ropePoints.RemoveAt(ropePoints.Count - 1);
                    anglesPositive.RemoveAt(anglesPositive.Count - 1);
                    //Debug.Log("Relaxing rope at " + ropePoints.Count + " (2)");
                }
            }
            oldPos1 = start;
            oldPos2 = end;
        }

    }

    private void KillTeam(GameObject player)
    {
        Debug.Log("KillTeam is working");
        gameObject.transform.parent.gameObject.GetComponent<Team>().AddPoints();
        Destroy(player.transform.parent.gameObject);
    }

    private bool IsAnglePositive(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 angle1 = (a - b).normalized;
        Vector3 angle2 = (c - b).normalized;
        return Mathf.Atan2(Vector3.Dot(Vector3.Cross(angle2, angle1), Vector3.up), Vector3.Dot(angle1, angle2)) > 0; //voodoo magic
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
