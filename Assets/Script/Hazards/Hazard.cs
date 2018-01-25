using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float timerLength;
    private float timer;
    private bool isActive;
    GameObject hazardPrefab;

    public virtual void Start()
    {
        timer = timerLength;
    }

    public virtual void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            if (isActive)
            {
                DeactivateTrap();
            }
            else
            {
                ActivateTrap();
            }
            ResetTimer();
        }
    }

    public virtual void ResetTimer()
    {
        timer = timerLength;
    }

    public virtual void ActivateTrap()
    {
        isActive = true;
    }

    public virtual void DeactivateTrap()
    {
        isActive = false;
    }

    public virtual void OnCollisionEnter(Collision other)
    {

    }

    public virtual void OnCollisionExit(Collision other)
    {

    }

    public virtual void OnCollisionStay(Collision other)
    {

    }
}
