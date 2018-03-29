using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer:    Nicholas Nguyen
//Date:         3/2/18
//Description:  Script that has method that shakes the camera
//              Does not need to be attached to camera, just make sure
//              its transform is set through public method
public class CameraShake : MonoBehaviour {

    public Transform camTransform;

    Vector3 originalPos;

    private void Awake()
    {
        //Sets the original camera position
        originalPos = camTransform.position;

        //Below is used for testing
        //StartCoroutine(shake(10,4));
    }

    /*
     * Method that shakes the camera
     * @param duration: the duration of the shake
     * @param shakeAmount: the intenity of the shaking
     */
    public IEnumerator shake(float duration, float shakeAmount)
    {
        //While loop for duration of shaking
        while(duration >= 0)
        {
            //Randomly shakes camera
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            //Decreases the duration of shaking
            duration -= Time.deltaTime;

            yield return null;
        }
        //Resets the camera to original position
        camTransform.localPosition = originalPos;

        yield return null;
    }
}
