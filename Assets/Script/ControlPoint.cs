using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:       Nick Arnieri
// Date:            11/3/2017
// Description:     Responsible for spawning and detecting collisions for the control point

public class ControlPoint : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject point;
    public Vector3 position;
    public int width, length, height;

    // Use this for initialization
    void Start()
    {
        if (GameManager.singleton.currentGame.GetType() == typeof(KingOfTheHill))
        {
            GameObject newObject = Instantiate(pointPrefab, position, Quaternion.identity, transform);
            point = newObject;
            point.AddComponent<PointCollision>();
            point.transform.localScale = new Vector3(width, height, length);
        }
    }
}

public class PointCollision : MonoBehaviour
{
    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (GameManager.singleton.hillRules)
    //    {
    //        GameManager.singleton.hillRules.ChangeOnPoint(collision.gameObject.transform.parent.gameObject.name);
    //    }
    //}

    //private void OnTriggerExit(Collider collision)
    //{
    //    if (GameManager.singleton.hillRules)
    //    {
    //        GameManager.singleton.hillRules.ChangeOffPoint(collision.gameObject.transform.parent.gameObject.name);
    //    }
    //}
}