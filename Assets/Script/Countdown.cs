using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Developer:   Trevor Angle
// Date:        3/22/2018
// Description: Starts displaying countdown after specified amount of time,
//              shows each countdown image for specified amount of time.
//              Can be restarted instantly with Reset() method. The images
//              don't glow/pulsate at all yet.

public class Countdown : MonoBehaviour {

    public float initialDelay;
    public float displayDuration;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite fight;

    private Image image;
    private float lastSpriteSwitchTime;
    private Sprite[] sprites;
    private int nextSpriteIndex;
    private float startTime;

    // Use this for initialization
    void Start () {
        /*Initializing private instance variables*/
        image = gameObject.GetComponent<Image>();
        lastSpriteSwitchTime = Time.time - displayDuration;
        sprites = new Sprite[] { three, two, one, fight };
        nextSpriteIndex = 0;
        startTime = Time.time;

        image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        // if it's time to start counting and an appropriate amount of time has passed since we last switched sprites
        if (Time.time - startTime >= initialDelay && Time.time - lastSpriteSwitchTime >= displayDuration) {
            // if we haven't gone through all the sprites yet
            if (nextSpriteIndex < sprites.Length) {
                image.enabled = true;
                lastSpriteSwitchTime = Time.time;
                image.sprite = sprites[nextSpriteIndex];
                image.SetNativeSize();
                nextSpriteIndex++;
            }
            else  {
                image.enabled = false;
            }
        }
	}

    public void Reset() {
        nextSpriteIndex = 0;
    }
}
