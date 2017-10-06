using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        8/28/17
// Description: This class represents a team of two players, and manages the rope connection
//              between them. Currently it is only responsible for recreating the rope whenever
//              it is destroyed.

// Points tracker added by Connor Pillsbury, 9/20/17

// Developer:   Kyle Aycock
// Date:        9/20/17
// Description: Team is now responsible for spawning players
//              Added methods for spawn points and resetting

public class Team : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject ropePrefab;
    public float ropeFormTime;
    public GameObject player1;
    public GameObject player2;
    public GameObject currentRope;
    public int points;

    private Vector3 spawn1;
    private Vector3 spawn2;
    private string controls1;
    private string controls2;
    private float rejoinTimer;
    private bool hasRope = false;

    // Use this for initialization
    void Start()
    {
        rejoinTimer = ropeFormTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (rejoinTimer > 0)// && !Rope.RaycastSegment(player1.transform.position, player2.transform.position, gameObject.layer))
            rejoinTimer -= Time.deltaTime;
        if (rejoinTimer <= 0)
        {
            if (currentRope == null)
            {
                if (hasRope)
                {
                    hasRope = false;
                    rejoinTimer = ropeFormTime;
                }
                else if (player1 && player2 && Vector3.Distance(player1.transform.position, player2.transform.position) <= ropePrefab.GetComponent<Rope>().maxRopeLength && !Rope.RaycastSegment(player1.transform.position, player2.transform.position, gameObject.layer))
                {
                    currentRope = Instantiate(ropePrefab, transform);
                    currentRope.GetComponent<Rope>().SetEndpoints(player1.transform, player2.transform);
                    hasRope = true;
                }
            }
        }

    }

    public void SetSpawnPoints(Vector3 first, Vector3 second)
    {
        spawn1 = first;
        spawn2 = second;
    }

    public void SpawnPlayer(int num)
    {
        if ((num == 1 && player1) || (num == 2 && player2)) return;
        Vector3 pos = (num == 1 ? spawn1 : spawn2);
        GameObject newPlayer = Instantiate(playerPrefab, pos, Quaternion.identity, transform);
        newPlayer.layer = gameObject.layer;

        if (num == 1)
        {
            player1 = newPlayer;
            player1.GetComponent<PlayerController>().SetControls("Vertical" + controls1, "Horizontal" + controls1);
        }
        else
        {
            player2 = newPlayer;
            player2.GetComponent<PlayerController>().SetControls("Vertical" + controls2, "Horizontal" + controls2);
        }
    }

    public void SetControls(string p1, string p2)
    {
        controls1 = p1;
        controls2 = p2;
    }

    public void ResetTeam()
    {
        if (currentRope) Destroy(currentRope);
        if (player1) Destroy(player1);
        if (player2) Destroy(player2);
        player1 = null;
        player2 = null;
        SpawnPlayer(1);
        SpawnPlayer(2);
    }

    public void AddPoints()
    {
        points++;
        Debug.Log(name + ": " + points);
    }

    public void KillTeam()
    {
        GameManager.singleton.KillTeam(this);
    }
}
