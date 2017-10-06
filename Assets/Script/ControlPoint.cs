using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject point;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Setup cap point
    /// </summary>
    public void Setup(Vector3 position)
    {
        GameObject newPoint = Instantiate(pointPrefab, position, Quaternion.identity, transform);
        newPoint.layer = gameObject.layer;
        point = newPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.singleton.hillRules.ChangeOnPoint(collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        GameManager.singleton.hillRules.ChangeOffPoint(collision.gameObject.name);
    }
}
