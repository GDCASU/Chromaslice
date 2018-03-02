using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Nick Arnieri
// Date:        1/26/2017
// Description: Base class for hazards

// Developer:   Nick Arnieri
// Date:        2/22/2017
// Description: Add collision events

public class Hazard : MonoBehaviour
{
    public float activeLength;
    public float inactiveLength;
    protected float timer;
    protected bool isActive;

    public virtual void Start()
    {
        timer = inactiveLength;
        isActive = false;
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
        timer = isActive ? activeLength : inactiveLength;
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

    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void OnTriggerExit(Collider other)
    {
        
    }

    public virtual void OnTriggerStay(Collider other)
    {
        
    }
}
