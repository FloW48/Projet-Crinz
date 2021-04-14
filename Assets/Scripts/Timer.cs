using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text textTimer;
    private int previousSec = -1;
    private float decalTime;
    // Start is called before the first frame update
    void Start()
    {
        textTimer.text = "Time : 0";
    }

    // Update is called once per frame
    void Update()
    {
        if((int)(Time.timeSinceLevelLoad-decalTime) != previousSec)
        {
            previousSec = (int)(Time.timeSinceLevelLoad-decalTime);
            textTimer.text = "Time : " + previousSec.ToString();
        }
    }

    void OnEnable()
    {
        decalTime = Time.timeSinceLevelLoad;
    }
}
