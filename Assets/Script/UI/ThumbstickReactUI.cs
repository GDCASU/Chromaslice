using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbstickReactUI : MonoBehaviour 
{
    public string reactAxisX;
    public string reactAxisY;
    public float scale;

    private Vector3 defaultPos;
	// Use this for initialization
	void Start () 
    {
        defaultPos = this.GetComponent<RectTransform>().anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {

        this.GetComponent<RectTransform>().anchoredPosition = defaultPos + new Vector3( Input.GetAxis(reactAxisX) * scale, Input.GetAxis(reactAxisY) * scale, 0);


	}
}
