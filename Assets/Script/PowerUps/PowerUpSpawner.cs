using UnityEngine;

/*
 * Description: This script allows designers to spawn power-ups in the game with a variety of methods.
 * A prefab has been created that allows simple drap and drop use
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 */

public class PowerUpSpawner : MonoBehaviour
{
    // Structure to hold Power-Up Data
    [System.Serializable]
    public struct PowerUpStruct
    {
        public bool flag;
        public float param1;
        public float param2;
        public float param3;
    }

    // Enumerations for various Spawning methods
    public enum SpawningMethod {Single, Random }

    // Enumeration for PwoerUp types
    public enum PowerUpType { Speed, Rope, Knockback, Incibility}

    // Fields that are always shown
    public SpawningMethod method;           // The spawning method of the Spawner
    public GameObject[] powerUpPrefabs;     // The prefabs of the powerups to create

    // Required for display in the inspector
    public bool continuousSpawn;
    public float spawnInterval;
    public bool speedSelect;
    public bool ropeSelect;
    public bool knockbackSelect;
    public bool invincibilitySelect;
    public PowerUpStruct speedStruct;
    public PowerUpStruct ropeStruct;
    public PowerUpStruct knockbackStruct;
    public PowerUpStruct invincibilityStruct;
    private PowerUpStruct[] structArray;

    // Property in SINGLE mode to mark which power-up to spawn
    public PowerUpType selection;

    // What is the spawner's current powerup
    private GameObject currentPowerUp;
    private float intervalTimer;

	void Start ()
    {
        currentPowerUp = null;
        intervalTimer = spawnInterval;

        // Add all PowerUpStructures to an array
        structArray = new PowerUpStruct[powerUpPrefabs.Length];
        structArray[0] = speedStruct;
        structArray[1] = ropeStruct;
        structArray[2] = knockbackStruct;
        structArray[3] = invincibilityStruct;
	}
	
	void Update ()
    {
		if(currentPowerUp == null)
        {
            if (intervalTimer > 0)
                intervalTimer -= Time.deltaTime;

            // Interval timer has ended
            else
            {
                // Random method
                if (method == SpawningMethod.Random)
                {
                    ChooseRandomPowerUp();
                }

                // Single method
                else if (method == SpawningMethod.Single)
                {
                    currentPowerUp = Instantiate(powerUpPrefabs[(int)selection], gameObject.transform);
                    currentPowerUp.GetComponent<PowerUp>().SetData(structArray[(int)selection].param1, structArray[(int)selection].param2, structArray[(int)selection].param3);
                }

                // Reset timer
                intervalTimer = spawnInterval;
            }
        }
	}

    // Chooses a random power-up that is allowed by the spawner
    private void ChooseRandomPowerUp()
    {
        int random = Random.Range(0, structArray.Length);

        while (structArray[random].flag == false)
            random = Random.Range(0, structArray.Length);

        currentPowerUp = Instantiate(powerUpPrefabs[random], gameObject.transform);

        // Set the custom data from the Inspector Property fields to the power-up's
        currentPowerUp.GetComponent<PowerUp>().SetData(structArray[random].param1, structArray[random].param2, structArray[random].param3);
    }
}