using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Nick Arnieri
// Date:        1/26/2017
// Description: Base class for hazards

public class Hazard : MonoBehaviour
{
    public float timerLength;
    protected float timer;
    protected bool isActive;

    public virtual void Start()
    {
        timer = timerLength;
    }

    public virtual void Update()
    {
        // Handles the timer and toggling the hazard on and off
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

    public virtual void OnTriggerEnter(Collider other)
    {
        // Only do the collison event if the hazard is active
        if (!isActive)
        {
            return;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        // Only do the collison event if the hazard is active
        if (!isActive)
        {
            return;
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        // Only do the collison event if the hazard is active
        if (!isActive)
        {
            return;
        }
    }
}
