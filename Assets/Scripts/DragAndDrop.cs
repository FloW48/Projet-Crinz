using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{


    bool moveAllowed;
    Collider2D col;
    float x;
    private GameObject blueSpike;
    private GameObject redSpike;


    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        col = GetComponent<Collider2D>();
        blueSpike = GameObject.FindGameObjectWithTag("BlueSpike");
        redSpike = GameObject.FindGameObjectWithTag("RedSpike");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMaster.isDead)
        {
            if (!GameMaster.twoPlayers)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    transform.position = new Vector2(x, touchPosition.y);
                }
            }
            else
            {
                if(Input.touchCount > 0)
                {

                    for(int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        if(touchPosition.x < -1)
                        {
                            blueSpike.transform.position = new Vector2(blueSpike.transform.position.x, touchPosition.y);
                        }
                        else
                        {
                            redSpike.transform.position = new Vector2(redSpike.transform.position.x, touchPosition.y);
                        }
                    }
                }
            }
        }
    }
}
