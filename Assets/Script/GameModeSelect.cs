using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelect : MonoBehaviour
{

    public Dropdown m_Dropdown;
    public GameObject EliminationPanel;
    public GameObject DeathmatchPanel;
    public GameObject SoccerPanel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Dropdown.value == 0)
        {
            EliminationPanel.SetActive(true);
            DeathmatchPanel.SetActive(false);
            SoccerPanel.SetActive(false);

        }
        if (m_Dropdown.value == 1)
        {
            EliminationPanel.SetActive(false);
            DeathmatchPanel.SetActive(true);
            SoccerPanel.SetActive(false);

        }
        if (m_Dropdown.value == 2)
        {
            EliminationPanel.SetActive(false);
            DeathmatchPanel.SetActive(false);
            SoccerPanel.SetActive(true);

        }

    }
}
