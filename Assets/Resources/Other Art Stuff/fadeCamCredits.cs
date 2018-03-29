using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeCamCredits : MonoBehaviour {

    public SpriteRenderer fade;
    private float alpha;
    private float timeCount;


    // Use this for initialization
    void Start () {
        alpha = 1.0f;
        timeCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        timeCount = timeCount + Time.deltaTime;

        if (timeCount < 2.0)
        {
            alpha = 1.0f -  timeCount * 0.5f;
        } else
        {
            if (timeCount > 102 && timeCount < 107)
            {
                alpha = (timeCount - 102f) * 0.2f;
            } else
            {
                if (timeCount > 2.0f && timeCount < 104) alpha = 0.0f;
                if (timeCount > 107f)
                {
                    alpha = 1.0f;
                    //TODO: Load Title Scene
                    SceneManager.LoadScene("Title", LoadSceneMode.Single);

                }

            }

        }
        fade.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        //print(timeCount); // testing counter
        //print(Time.deltaTime); // testing deltaTime
	}
}
