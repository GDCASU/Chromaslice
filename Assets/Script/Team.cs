﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

// Developer:   Connor Pillsbury
// Date:        9/24/17
// Description: Team now has a invincibility delay in the spawnTimer


public class Team : NetworkBehaviour
{
    public GameObject CurrentPowerUp
    {
        get { return currentPowerUp; }
        set { currentPowerUp = value; }
    }

    public GameObject playerPrefab;
    public GameObject ropePrefab;
    public GameObject invincibilityParticlePrefab;  // a prefab that has a particle system component
    public float ropeFormTime;
    public GameObject currentRope;
    public int points;
    public float spawnTimer;
    public GameObject player1;
    public GameObject player2;

    private Vector3 spawn1;
    private Vector3 spawn2;
    private float rejoinTimer;
    private bool hasRope = false;
    private GameObject currentPowerUp;

    [SyncVar]
    public NetworkInstanceId player1Id;
    [SyncVar]
    public NetworkInstanceId player2Id;

    [SyncVar]
    public int controller1;
    [SyncVar]
    public int controller2;
    [SyncVar]
    private Color color1;
    [SyncVar]
    private Color color2;

    // Use this for initialization
    void Start()
    {
        rejoinTimer = ropeFormTime;
        currentPowerUp = null;
    }

    public override void OnStartClient()
    {
        if (!player1Id.IsEmpty()) SetupPlayer(player1Id, controller1, 1);
        if (!player2Id.IsEmpty()) SetupPlayer(player2Id, controller2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;
        if (!IsInvincibleOver())
        {
            spawnTimer -= Time.deltaTime;

        }
        else spawnTimer = 0;
        //Debug.Log(spawnTimer);


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
                    currentRope.GetComponent<Rope>().parentId = GetComponent<NetworkIdentity>().netId;
                    NetworkServer.Spawn(currentRope);
                    hasRope = true;
                }
            }
        }

    }

    /// <summary>
    /// Checks to see if the players are still invincible durring the spawn time 
    /// in the start of the round
    /// </summary>
    /// <returns></returns>
    public bool IsInvincibleOver()
    {
        return spawnTimer <= 0;
    }

    //clients don't need to know about this
    [Server]
    public void SetSpawnPoints(Vector3 first, Vector3 second)
    {
        spawn1 = first;
        spawn2 = second;
    }

    [Server]
    public GameObject SpawnPlayer(Player ply)
    {
        Debug.Log(ply.controllerId);
        int num = ply.playerId % 2 + 1;
        if ((num == 1 && player1) || (num == 2 && player2)) return null;
        Vector3 pos = (num == 1 ? spawn1 : spawn2);
        GameObject newPlayer = Instantiate(playerPrefab, pos, Quaternion.identity, transform);

        newPlayer.layer = gameObject.layer;

        spawnTimer = GameManager.singleton.spawnTimer;

        GameObject invinciblePrefab = Instantiate(invincibilityParticlePrefab, newPlayer.transform.position, Quaternion.identity, newPlayer.transform);
        var main = invinciblePrefab.GetComponent<ParticleSystem>().main;
        //main.duration = spawnTimer;
        main.startLifetime = spawnTimer;
        Destroy(invinciblePrefab, spawnTimer);

        NetworkServer.Spawn(newPlayer);
        if (num == 1)
        {
            player1Id = newPlayer.GetComponent<NetworkIdentity>().netId;
            controller1 = ply.controllerId;
            RpcSetupPlayer(newPlayer.GetComponent<NetworkIdentity>().netId, controller1, num);
        }
        else
        {
            player2Id = newPlayer.GetComponent<NetworkIdentity>().netId;
            controller2 = ply.controllerId;
            RpcSetupPlayer(newPlayer.GetComponent<NetworkIdentity>().netId, controller2, num);
        }

        return newPlayer;
    }


    [ClientRpc]
    public void RpcSetupPlayer(NetworkInstanceId ply, int control, int num)
    {
        SetupPlayer(ply, control, num);
    }

    public void SetupPlayer(NetworkInstanceId ply, int control, int num)
    {
        if (num == 1)
        {
            player1Id = ply;
            controller1 = control;
            player1 = ClientScene.FindLocalObject(ply);
            ApplyColor(player1, color1);
            player1.GetComponent<PlayerController>().SetControls(controller1+1);
            player1.transform.SetParent(transform);
        }
        else
        {
            player2Id = ply;
            controller2 = control;
            player2 = ClientScene.FindLocalObject(ply);
            ApplyColor(player2, color2);
            player2.GetComponent<PlayerController>().SetControls(controller2+1);
            player2.transform.SetParent(transform);
        }
    }

    public void SetControls(int p1controller, int p2controller)
    {
        controller1 = p1controller;
        controller2 = p2controller;
    }

    public void SetColors(Color c1, Color c2)
    {
        color1 = c1;
        color2 = c2;
    }

    public void ApplyColor(GameObject player, Color color)
    {
        Renderer[] rends = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            foreach (Material m in r.materials)
            {
                if (m.HasProperty("_Color"))
                    m.color = Color.Lerp(m.color, color, 0.005f);
                else if (m.HasProperty("_TintColor"))
                    m.SetColor("_TintColor", Color.Lerp(m.GetColor("_TintColor"), color, 0.005f));
            }
        player.GetComponentInChildren<Light>().color = Color.Lerp(player.GetComponentInChildren<Light>().color, color, 0.005f);
    }

    public void ResetTeam()
    {
        if (currentRope) Destroy(currentRope);
        if (player1) Destroy(player1);
        if (player2) Destroy(player2);
        player1 = null;
        player2 = null;
        //SpawnPlayer(1);
        //SpawnPlayer(2);
    }

    public void AddPoints()
    {
        points++;
        Debug.Log(name + ": " + points);
    }

    public void KillTeam()
    {
        // Check if the team doesn't have invincibility
        if (!(currentPowerUp != null && currentPowerUp.GetComponent<InvincibilityPowerUp>().isActive))
            GameManager.singleton.KillTeam(this);
    }
}
