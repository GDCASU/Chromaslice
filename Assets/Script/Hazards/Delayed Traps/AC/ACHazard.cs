using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Tyler Cole
// Date:        2/19/2018
// Description: Class for the A/C delayed hazard

public class ACHazard : Hazard {

    public GameObject hitbox;

    // Initalize timer
    public override void Start()
    {
        base.Start();
    }

    // Handles timer and toggling hazard status
    public override void Update()
    {
        base.Update();
    }

    // Resets the timer of the trap
    public override void ResetTimer()
    {
        base.ResetTimer();
    }

    // Activates the trap
    public override void ActivateTrap()
    {
        base.ActivateTrap();
        hitbox.SetActive(true);
    }

    // Deactivates the trap
    public override void DeactivateTrap()
    {
        base.DeactivateTrap();
        hitbox.SetActive(false);
    }

    // Behavior for when another object collides with this object
    // <param name="other">Game object colliding with</param>
    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    // Behavior for when another object stops colliding with this object
    // <param name="other">Game object colliding with</param>
    public override void OnCollisionExit(Collision other)
    {
        base.OnCollisionExit(other);
    }

    // Behavior for when another object is touching this object
    // <param name="other">Game object colliding with</param>
    public override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
    }

    // Behavior for when another objects trigger touches this object
    // <param name="other">Game object colliding with</param>
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    // Behavior for when another objects trigger stops touching this object
    // <param name="other">Game object colliding with</param>
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    // Behavior for when another objects trigger is touching this object
    // <param name="other">Game object colliding with</param>
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }
}