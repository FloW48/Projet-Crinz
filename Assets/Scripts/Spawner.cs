using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ball;
    private float spawnRate = 2.4f;
    private float minSpawnRate = 1.2f;
    private float secNextSpawn = 0f;
    private int secNextDifficulty = 0;
    private int secToNextDifficulty = 5;
    private int timeTillMaxSpeed = 150;
    private float decalTime;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.timeSinceLevelLoad - decalTime) >= secNextSpawn)
        {
            secNextSpawn += spawnRate;
            GameObject newBall = Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);

            if (GameMaster.twoPlayers)
            {
                GameObject newBallRight = Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
                Ball ballLeft = newBall.GetComponent<Ball>();
                Ball ballRight = newBallRight.GetComponent<Ball>();
                ballRight.angle = Random.Range(-Mathf.PI / 3, Mathf.PI / 3);
                ballLeft.angle = Random.Range(2f * Mathf.PI / 3, 4f * Mathf.PI / 3);

                if(Random.Range(0, 8) < 1)
                {
                    ballLeft.setColor(new Color(0.18f, 0.18f, 0.18f, 1f));
                    ballRight.setColor(new Color(0.18f, 0.18f, 0.18f, 1f));
                    ballLeft.tag = "Ball noire";
                    ballRight.tag = "Ball noire";
                }
                else
                {
                    if (Random.Range(0, 6) == 5)
                    {
                        ballLeft.tag = "BallRouge";
                        ballRight.tag = "BallBleue";
                        ballRight.setColor(new Color(0f, 0.5676286f, 1f, 1f));
                        ballLeft.setColor(Color.red);
                    }
                    else
                    {
                        ballLeft.tag = "BallBleue";
                        ballRight.tag = "BallRouge";
                        ballLeft.setColor(new Color(0f, 0.5676286f, 1f, 1f));
                        ballRight.setColor(Color.red);
                    }
                }

                float speed = Random.Range(3f, 6f);
                ballLeft.speed = speed;
                ballRight.speed = speed;
            }
        }
        if ((int)(Time.timeSinceLevelLoad - decalTime) >= secNextDifficulty)
        {
            secNextDifficulty += secToNextDifficulty;
            spawnRate = Mathf.Lerp(spawnRate, minSpawnRate, getDiffPercent());
        }
    }

    float getDiffPercent()
    {
        return Mathf.Clamp01((Time.timeSinceLevelLoad - decalTime) / timeTillMaxSpeed);
    }

    void OnEnable()
    {
        decalTime = Time.timeSinceLevelLoad;
    }
}
