using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour 
{
    public Image currentImage;      // the image that's currently displayed
    public Sprite[] images;         // the images the carousel cycles through

    public Text textLabel;
    public string[] labels;         // corresponding labels for the images

    public Button leftArrow;
    public Button rightArrow;

    private int currentLocation;    // current index of the carousel

    public delegate void OnChange();       // when the carousel is changed in someway
    public OnChange Traverse;       // when traversing through the carousel

	// Use this for initialization
	void Start () 
    {
        Traverse += UpdateImage;
        Traverse += UpdateLabel;

        currentLocation = 0;
        UpdateImage();
        UpdateLabel();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    // called by the right arrow button
    public void GoRight()
    {
        currentLocation++;

        currentLocation %= images.Length;

        if (Traverse != null)
            Traverse();
    }

    // called by the left arrow button 
    public void GoLeft()
    {
        currentLocation--;

        if (currentLocation < 0)
            currentLocation = images.Length - 1;
        
        currentLocation %= images.Length;

        if (Traverse != null)
            Traverse();
    }

    void UpdateImage()
    {
        currentImage.sprite = images[currentLocation];
    }

    void UpdateLabel()
    {
        textLabel.text = labels[currentLocation];
    }


}
