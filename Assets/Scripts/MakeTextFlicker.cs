using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeTextFlicker : MonoBehaviour
{
    private Text text;
    private float alpha;
    private bool goingUp;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        goingUp = true;
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha <= 0f)
        {
            goingUp = true;
        }
        else if (alpha >= 1)
        {
            goingUp = false;
        }
        if (alpha < 0.5f && goingUp)
        {
            alpha += 0.01f;
        }
        else if (alpha < 0.5f && !goingUp)
        {
            alpha -= 0.01f;
        }
        else if (alpha >= 0.5f && goingUp)
        {
            alpha += 0.005f;
        }
        else if (alpha >= 0.5f && !goingUp)
        {
            alpha -= 0.005f;
        }
        text.color = new Color(1f, 1f, 1f, alpha);
    }
}
