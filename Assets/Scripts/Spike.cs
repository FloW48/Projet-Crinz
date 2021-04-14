using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spike : MonoBehaviour
{
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
        Color ballColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        if(collision.tag == "BallBleue")
        {
            if (ballColor.Equals(gameObject.GetComponent<SpriteRenderer>().color))
            {
                if (!GameMaster.twoPlayers)
                {
                    PlayerPrefs.SetInt("ballBleuePop", PlayerPrefs.GetInt("ballBleuePop")+1);
                }
                Destroy(collision.gameObject);
            }
            else
            {
                if (!GameMaster.twoPlayers)
                {
                    PlayerPrefs.SetInt("ballBleueDeath", PlayerPrefs.GetInt("ballBleueDeath") + 1);
                    StartCoroutine(highlightDeathBall(collision.gameObject));
                    StartCoroutine(GameMaster.endGame());
                    Time.timeScale = 0.01f;
                }
                else
                {
                    if(GameMaster.lifeBlue >= 0)
                    {
                        GameMaster.lifeBlue -= 1;
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        GameMaster.winner = "Red";
                        PlayerPrefs.SetInt("ballBleueDeath", PlayerPrefs.GetInt("ballBleueDeath") + 1);
                        StartCoroutine(highlightDeathBall(collision.gameObject));
                        StartCoroutine(GameMaster.endGame());
                        Time.timeScale = 0.01f;
                    }
                }
            }
        }
        else if (collision.tag == "BallRouge")
        {
            if(ballColor.Equals(gameObject.GetComponent<SpriteRenderer>().color))
            {
                if (!GameMaster.twoPlayers)
                {
                    PlayerPrefs.SetInt("ballRougePop", PlayerPrefs.GetInt("ballRougePop") + 1);
                }
                Destroy(collision.gameObject);
            }
            else
            {
                if (!GameMaster.twoPlayers)
                {
                    PlayerPrefs.SetInt("ballRougeDeath", PlayerPrefs.GetInt("ballRougeDeath") + 1);
                    StartCoroutine(highlightDeathBall(collision.gameObject));
                    StartCoroutine(GameMaster.endGame());
                    Time.timeScale = 0.01f;
                }
                else
                {
                    if (GameMaster.lifeRed > 0)
                    {
                        GameMaster.lifeRed -= 1;
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        GameMaster.winner = "Blue";
                        PlayerPrefs.SetInt("ballRougeDeath", PlayerPrefs.GetInt("ballRougeDeath") + 1);
                        StartCoroutine(highlightDeathBall(collision.gameObject));
                        StartCoroutine(GameMaster.endGame());
                        Time.timeScale = 0.01f;
                    }
                }
            }
        }
        else if (collision.tag == "Ball noire")
        {
            if(gameObject.GetComponent<SpriteRenderer>().color == Color.red){
                GameMaster.winner = "Blue";
            }
            else
            {
                GameMaster.winner = "Red";
            }
            PlayerPrefs.SetInt("nbMortSurBallNoir", PlayerPrefs.GetInt("nbMortSurBallNoir") + 1);
            StartCoroutine(highlightDeathBall(collision.gameObject));
            StartCoroutine(GameMaster.endGame());
            Time.timeScale = 0.01f;
        }
        if (GameMaster.twoPlayers)
        {
            GameMaster.updateScore();
        }
    }

    IEnumerator highlightDeathBall(GameObject ball)
    {
        Debug.Log(ball.transform.localScale.x);
        float ballSize = ball.transform.localScale.x;
        Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
        bool getBigger = false;
        while (ball != null)
        {
            if (getBigger)
            {
                Debug.Log("big");
                ball.transform.localScale += scale;
                if(ball.transform.localScale.x > ballSize*1.15)
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
