using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour {

    public GameObject ropePrefab;
    public GameObject playerPrefab;
    public PhysicMaterial playerMaterial;

    private float ropeStartLength;
    private float playerStartSpeed;
    private float startBounciness;

    public void Start()
    {
        ropeStartLength = ropePrefab.GetComponent<Rope>().maxRopeLength;
        playerStartSpeed = playerPrefab.GetComponent<PlayerController>().maxSpeed;
        startBounciness = playerMaterial.bounciness;
    }

	public void ChangeSpeed(float newSpeed)
    {
        playerPrefab.GetComponent<PlayerController>().maxSpeed = newSpeed;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            go.GetComponent<PlayerController>().maxSpeed = newSpeed;
    }

    public void SetRopeLength(float newLength)
    {
        ropePrefab.GetComponent<Rope>().maxRopeLength = newLength;
        foreach (Rope r in FindObjectsOfType<Rope>())
            r.maxRopeLength = newLength;
    }

    public void SetBounciness(float newBouncy)
    {
        playerMaterial.bounciness = newBouncy;
    }

    public void OnDestroy()
    {
        ropePrefab.GetComponent<Rope>().maxRopeLength = ropeStartLength;
        playerPrefab.GetComponent<PlayerController>().maxSpeed = playerStartSpeed;
        playerMaterial.bounciness = startBounciness;
    }
}
