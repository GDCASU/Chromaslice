using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsMenu : MonoBehaviour {

    public Dropdown m_Dropdown;
    public GameObject P1Panel;
    public GameObject P2Panel;
    public GameObject P3Panel;
    public GameObject P4Panel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Dropdown.value == 0)
        {
            P1Panel.SetActive(true);
            P2Panel.SetActive(false);
            P3Panel.SetActive(false);
            P4Panel.SetActive(false);
        }
        if (m_Dropdown.value == 1)
        {
            P1Panel.SetActive(false);
            P2Panel.SetActive(true);
            P3Panel.SetActive(false);
            P4Panel.SetActive(false);
        }
        if (m_Dropdown.value == 2)
        {
            P1Panel.SetActive(false);
            P2Panel.SetActive(false);
            P3Panel.SetActive(true);
            P4Panel.SetActive(false);
        }
        if (m_Dropdown.value == 3)
        {
            P1Panel.SetActive(false);
            P2Panel.SetActive(false);
            P3Panel.SetActive(false);
            P4Panel.SetActive(true);
        }
    }
}



