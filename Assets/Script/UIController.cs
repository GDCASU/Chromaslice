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
        if (Input.GetAxis("Vertical1") != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(defaultButtonSelected);

            SoundManager.singleton.PlaySoundInstance("MenuSelect");
            Debug.Log("Vertical1");
        }
        if (Input.GetAxis("Horizontal1") != 0) Debug.Log("Horizontal1");
        if (Input.GetAxis("Vertical1.5") != 0) Debug.Log("Vertical1.5");
        if (Input.GetAxis("Horizontal1.5") != 0) Debug.Log("Horizontal1.5");
        if (Input.GetAxis("Horizontal2") != 0) Debug.Log("Horizontal2");
        if (Input.GetAxis("Vertical2") != 0) Debug.Log("Vertical2");
        if (Input.GetAxis("Horizontal2.5") != 0) Debug.Log("Horizontal2.5");
        if (Input.GetAxis("Vertical2.5") != 0) Debug.Log("Vertical2.5");

        if (Input.GetKeyDown(KeyCode.JoystickButton0)) Debug.Log(KeyCode.JoystickButton0.GetHashCode());
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) Debug.Log(KeyCode.JoystickButton1.GetHashCode());
        if (Input.GetKeyDown(KeyCode.JoystickButton2)) Debug.Log("JoystickButton2");
        if (Input.GetKeyDown(KeyCode.JoystickButton3)) Debug.Log("JoystickButton3");
        if (Input.GetKeyDown(KeyCode.JoystickButton4)) Debug.Log("JoystickButton4");
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) Debug.Log("JoystickButton5");
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) Debug.Log("JoystickButton6");
        if (Input.GetKeyDown(KeyCode.JoystickButton7)) Debug.Log("JoystickButton7");
        if (Input.GetKeyDown(KeyCode.JoystickButton8)) Debug.Log("JoystickButton8");
        if (Input.GetKeyDown(KeyCode.JoystickButton9)) Debug.Log("JoystickButton9");
        if (Input.GetKeyDown(KeyCode.JoystickButton10)) Debug.Log("JoystickButton10");

    }

    public void SetDefaultButton(GameObject button)
    {
        defaultButtonSelected = button;
        EventSystem.current.SetSelectedGameObject(defaultButtonSelected);   
    }
}
