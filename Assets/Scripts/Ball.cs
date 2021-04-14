using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public GameObject ball;
    public float angle;
    public int life = 1;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameMaster.twoPlayers)
        {
            speed = Random.Range(3f, 6f);
            if (Random.Range(0, 2) < 1)
            {
                angle = Random.Range(2f * Mathf.PI / 3, 4f * Mathf.PI / 3);
            }
            else
            {
                angle = Random.Range(-Mathf.PI / 3, Mathf.PI / 3);
            }

            int randomInt = Random.Range(0, 11);
            if (randomInt <= 4)
            {
                color = Color.red;
                gameObject.tag = "BallRouge";
            }
            else if (randomInt <= 9)
            {
                color = new Color(0f, 0.5676286f, 1f, 1f);
                gameObject.tag = "BallBleue";
            }
            else
            {
                color = new Color(0.18f, 0.18f, 0.18f, 1f);
                gameObject.tag = "Ball noire";
            }
            setColor(color);
        }
    }

    public void setColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextLocation(), speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        SoundManager.PlaySound();
    }

    Vector2 nextLocation()
    {
        return new Vector2(transform.position.x + Mathf.Cos(angle), transform.position.y + Mathf.Sin(angle));
    }

    void inverseAngle(bool[] parameters)
    {
        bool isTopOrBottom = parameters[0];
        bool isLeft = parameters[1];
        if (isTopOrBottom)
        {
            angle = -angle;
        }
        else
        {
            angle = Mathf.PI - angle;
        }
    }

    void decrementLife()
    {
        life = life - 1;
    }

    void checkIfDestroys(GameObject ball)
    {
        if (life == 0)
        {
            PlayerPrefs.SetInt("nbBallNoire", PlayerPrefs.GetInt("nbBallNoire"));
            Destroy(ball);
        }
        else
        {
            decrementLife();
        }

    }

}
