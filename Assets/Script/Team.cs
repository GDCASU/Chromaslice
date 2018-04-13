using System.Collections;
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

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: Changed variables to syncvars and reworked spawning/respawning
//              to work with networking

public class Team : NetworkBehaviour
{
    public GameObject CurrentPowerUp
    {
        get { return currentPowerUp; }
        set
        {
            if (currentPowerUp != null)
                currentPowerUp.GetComponent<PowerUp>().RemovePowerUp();
            currentPowerUp = value;
        }
    }

    public GameObject playerPrefab;
    public GameObject ropePrefab;
    public GameObject invincibilityParticlePrefab;  // a prefab that has a particle system component
    public float ropeFormTime;
    public GameObject currentRope;
    public GameObject player1;
    public GameObject player2;

    public GameObject spawnParticlePrefabRed;
    public GameObject deathParticlePrefabRed;
    public GameObject spawnParticlePrefabBlue;
    public GameObject deathParticlePrefabBlue;

    [SyncVar]
    private Vector3 spawn1;
    [SyncVar]
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
        if (!isServer)
            return;

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
                else if (GameManager.singleton.currentGame.IsRoundActive && player1.activeSelf && player2.activeSelf && Vector3.Distance(player1.transform.position, player2.transform.position) <= ropePrefab.GetComponent<Rope>().maxRopeLength && !Rope.RaycastSegment(player1.transform.position, player2.transform.position, gameObject.layer))
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
        Debug.Log("Spawning player " + ply.controllerId + " on team " + ply.team);
        CanvasLog.instance.Log("Spawning player " + ply.controllerId + " on team " + ply.team);
        int num = ply.playerId % 2 + 1;
        if ((num == 1 && player1) || (num == 2 && player2)) return null;
        Vector3 pos = (num == 1 ? spawn1 : spawn2);
        GameObject newPlayer = Instantiate(playerPrefab, pos, Quaternion.identity, transform);
        newPlayer.layer = gameObject.layer;


        newPlayer.transform.localScale = Vector3.zero;
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
            player1.GetComponent<PlayerController>().SetControls(controller1 + 1);
            player1.transform.SetParent(transform);
        }
        else
        {
            player2Id = ply;
            controller2 = control;
            player2 = ClientScene.FindLocalObject(ply);
            ApplyColor(player2, color2);
            player2.GetComponent<PlayerController>().SetControls(controller2 + 1);
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

    // Removes current power up (if any), resets each player and adds invincibility particles for beginning of each round
    public void ResetTeam()
    {
        if (currentPowerUp != null)
        {
            currentPowerUp.GetComponent<PowerUp>().RemovePowerUp();
            currentPowerUp = null;
        }

        if (currentRope)
            Destroy(currentRope);

        if (player1)
        {
            ResetPlayer(player1);
            AddInvincibilityEffect(player1, GameConstants.TimeBeforeRound);
        }

        if (player2)
        {
            ResetPlayer(player2);
            AddInvincibilityEffect(player2, GameConstants.TimeBeforeRound);
        }
    }

    // Resets and reactivates the player. Can be called by itself or another function (ResetTeam)
    public void ResetPlayer(GameObject player)
    {
        if (player == player1)
            player.transform.position = spawn1;
        else
            player.transform.position = spawn2;

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        AddSpawnEffect(player, 1.5f);
        player.GetComponent<PlayerController>().TriggerSpawn();
        player.SetActive(true);
    }

    [ClientRpc]
    public void RpcResetTeam()
    {
        ResetTeam();
    }

    // Adds the invincibility particle effect to the player game object and is destroyed when finished
    public void AddInvincibilityEffect(GameObject player, float duration)
    {
        var main = invincibilityParticlePrefab.GetComponent<ParticleSystem>().main;
        main.duration = duration;
        GameObject instance = Instantiate(invincibilityParticlePrefab, player.transform.position, Quaternion.identity, player.transform);
        Destroy(instance, duration);
    }

    public void AddSpawnEffect(GameObject player, float duration)
    {
        var main = invincibilityParticlePrefab.GetComponent<ParticleSystem>().main;
        main.duration = duration;
        GameObject instance = Instantiate(player.transform.root.name == "Team 0" ? spawnParticlePrefabRed : spawnParticlePrefabBlue, player.transform.position, Quaternion.identity, player.transform);
        Destroy(instance, duration);
    }

    // This function should probably be renamed to KillPlayer instead, we'll see
    // This function is called by the rope script when colliding with a player, and passes the player to kill as an argument
    public void KillTeam(GameObject player)
    {
        // Do not kill while carrying instance of invincibility prefab
        if (player.GetComponentInChildren<ParticleSystem>() && player.GetComponentInChildren<ParticleSystem>().gameObject.name == invincibilityParticlePrefab.name + "(Clone)")
            return;

        if (currentRope)
            Destroy(currentRope);

        // Players are simply deactivated and reactivated since deleting/respawning would complicate networking code
        player.SetActive(false);

        RpcKill(player);
        GameManager.singleton.KillTeam(player);
    }

    [ClientRpc]
    public void RpcKill(GameObject player)
    {
        // Instantiate particle effects
        GameObject explosion = Instantiate(player.transform.root.name == "Team 0" ? deathParticlePrefabRed : deathParticlePrefabBlue, player.transform.position, Quaternion.identity);
        Destroy(explosion, 5.0f);

        if (currentRope)
            Destroy(currentRope);

        // Players are simply deactivated and reactivated since deleting/respawning would complicate networking code
        player.SetActive(false);
    }
}