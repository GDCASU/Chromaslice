using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    public float displayDuration;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite fight;

    private Image image;
    private float lastSpriteSwitchTime;
    private Sprite[] sprites;
    private int nextSpriteIndex;

    // Use this for initialization
    void Start () {
        /*Initializing private instance variables*/
        image = gameObject.GetComponent<Image>();
        lastSpriteSwitchTime = Time.time;
        sprites = new Sprite[] { three, two, one, fight };
        nextSpriteIndex = 0;

        image.sprite = three;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpriteSwitchTime >= displayDuration) {
            lastSpriteSwitchTime = Time.time;
            image.sprite = sprites[nextSpriteIndex];
            nextSpriteIndex++;
        }
	}
}
