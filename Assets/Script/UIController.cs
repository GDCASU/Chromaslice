using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    public GameObject defaultButtonSelected;
	
	// Update is called once per frame
	void Update () 
    {
        // this won't work with networking, idk lol
        if (Input.GetAxis("Vertical1") != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(defaultButtonSelected);

            SoundManager.singleton.PlaySoundInstance("MenuSelect");
            Debug.Log("lol");
        }

    }

    public void SetDefaultButton(GameObject button)
    {
        defaultButtonSelected = button;
        EventSystem.current.SetSelectedGameObject(defaultButtonSelected);   
    }
}
