using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundNumberHolder : MonoBehaviour {

    public Text roundNumText;

    int value = 1;

    public void IncreaseVal()
    {
        value++;
        roundNumText.text = value.ToString();
    }

    public void DecreaseVal()
    {
        if (value > 1)
            value--;

        roundNumText.text = value.ToString();
    }

}
