using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isTopOrBottom;
    public bool isLeft;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball noire" && !isTopOrBottom)
        {
            collision.gameObject.SendMessage("checkIfDestroys", collision.gameObject);
        }
        if (GameMaster.twoPlayers)
        {
            if(collision.tag == "BallBleue" && isLeft)
            {
                GameMaster.lifeBlue -= 1;
                if(GameMaster.lifeBlue == 0)
                {
                    GameMaster.winner = "Red";
                    StartCoroutine(highlightDeathBall(collision.gameObject));
                    StartCoroutine(GameMaster.endGame());
                    Time.timeScale = 0.01f;
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
            else if(collision.tag == "BallRouge" && !isLeft && !isTopOrBottom)
            {
                GameMaster.lifeRed -= 1;
                if(GameMaster.lifeRed == 0)
                {
                    GameMaster.winner = "Blue";
                    StartCoroutine(highlightDeathBall(collision.gameObject));
                    StartCoroutine(GameMaster.endGame());
                    Time.timeScale = 0.01f;
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
            GameMaster.updateScore();
        }

        bool[] parameters = { isTopOrBottom, isLeft };
        collision.gameObject.SendMessage("inverseAngle", parameters);
    }

    IEnumerator highlightDeathBall(GameObject ball)
    {
        float ballSize = ball.transform.localScale.x;
        Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
        bool getBigger = false;
        while (ball != null)
        {
            if (getBigger)
            {
                Debug.Log("big");
                ball.transform.localScale += scale;
                if (ball.transform.localScale.x > ballSize * 1.15)
                {
                    getBigger = false;
                }
            }
            else
            {
                Debug.Log("small");
                ball.transform.localScale -= scale;
                if (ball.transform.localScale.x < ballSize * 0.8695)
                {
                    getBigger = true;
                }
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
