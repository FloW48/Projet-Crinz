using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Text textHighScore;
    public Text OnePlayerText;
    public Text TwoPlayersText;
    public GameObject UIMenu;
    public GameObject timerPanel;
    public GameObject scorePanel;
    public GameObject spawner;
    public Text textStats;
    public GameObject PanelStats;
    public GameObject panelTutorial;
    public GameObject panelTutorialOne;
    public GameObject panelTutorialTwo;
    public static bool canRevive;
    public static bool revive;
    public static GameObject panelEndGame;
    private bool statsShow;
    private string statsString;
    private bool tutorialShow;
    internal static bool finishedWatching;
    public static bool twoPlayers;
    public static int lifeBlue;
    public static int lifeRed;
    private static bool skip;
    private static float countSkip;
    public static bool isDead;
    private static Color selectedColor = new Color(1,1,1);
    private static Color notSelectedColor = new Color(0.31f,0.31f,0.31f);
    public static Text wonTwoPlayers;
    public static string winner;
    public static GameObject progressBar;
    // Start is called before the first frame update
    void Start()
    {
        int tp = PlayerPrefs.GetInt("TwoPlayers");
        twoPlayers = tp == 0 ? false : true;
        GameMaster.panelEndGame = GameObject.FindGameObjectWithTag("PanelEndGame");
        GameMaster.wonTwoPlayers = GameObject.FindGameObjectWithTag("TextWonTwoPlayers").GetComponent<Text>();
        GameMaster.progressBar = GameObject.FindGameObjectWithTag("ProgressBar");
        wonTwoPlayers.gameObject.SetActive(false);
        panelEndGame.SetActive(false);
        canRevive = true;
        revive = false;
        statsShow = false;
        textHighScore.text = "Highscore : " + PlayerPrefs.GetInt("highscore").ToString();
        if(PlayerPrefs.GetInt("firstPlay") == 0)
        {
            panelTutorial.SetActive(true);
            panelTutorialOne.SetActive(true);
            tutorialShow = true;
            UIMenu.SetActive(false);
            PlayerPrefs.SetInt("firstPlay", 1);
        }
        lifeBlue = 3;
        lifeRed = 3;
        skip = false;
        countSkip = 0;
        isDead = false;
        updateColorTwoPlayersText();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("TwoPlayers", 0);
    }

    public void toggleOneTwoPlayers()
    {
        twoPlayers = !twoPlayers;
        PlayerPrefs.SetInt("TwoPlayers", twoPlayers ? 1 : 0);
        updateColorTwoPlayersText();
    }

    public void updateColorTwoPlayersText()
    {
        int twoPlayers = PlayerPrefs.GetInt("TwoPlayers");
        if (twoPlayers == 1)
        {
            OnePlayerText.color = notSelectedColor;
            TwoPlayersText.color = selectedColor;
        }
        else
        {
            OnePlayerText.color = selectedColor;
            TwoPlayersText.color = notSelectedColor;
        }
    }

    public void toggleTutorial()
    {
        if (tutorialShow)
        {
            panelTutorialTwo.SetActive(false);
            panelTutorial.SetActive(false);
            UIMenu.SetActive(true);
            tutorialShow = !tutorialShow;
        }
        else
        {
            panelTutorial.SetActive(true);
            panelTutorialOne.SetActive(true);
            UIMenu.SetActive(false);
            tutorialShow = !tutorialShow;
        }
    }

    public void showTutorialTwoPlayers()
    {
        panelTutorialOne.SetActive(false);
        panelTutorialTwo.SetActive(true);
    }

    public void toggleStats()
    {
        if (statsShow) //Enlever
        {
            PanelStats.SetActive(false);
            statsShow = !statsShow;
        }
        else //show
        {
            PanelStats.SetActive(true);
            statsString = "Games : "+PlayerPrefs.GetInt("nbGame").ToString() + "\n" +
                          "Average on 10 games : "+PlayerPrefs.GetFloat("moyenne").ToString() + "\n" +
                          "Highscore : "+PlayerPrefs.GetInt("highscore").ToString() + "\n" +
                          "Blue balls popped : "+PlayerPrefs.GetInt("ballBleuePop").ToString() + "\n" +
                          "Red balls popped : "+PlayerPrefs.GetInt("ballRougePop").ToString() + "\n" +
                          "Black balls dodged : "+PlayerPrefs.GetInt("nbBallNoire").ToString() + "\n" +
                          "Death on blue balls : "+PlayerPrefs.GetInt("ballBleueDeath").ToString() + "\n" +
                          "Death on red balls : "+PlayerPrefs.GetInt("ballRougeDeath").ToString() + "\n" +
                          "Death on black balls : "+PlayerPrefs.GetInt("nbMortSurBallNoir").ToString();
            textStats.text = statsString;
            statsShow = !statsShow;
        }
    }

    public static IEnumerator endGame()
    {
        if (GameMaster.twoPlayers)
        {
            wonTwoPlayers.text = winner+" won the game";
            wonTwoPlayers.gameObject.SetActive(true);
        }
        isDead = true;
        yield return new WaitForSecondsRealtime(2f);
        GameObject[] ballsBleue = GameObject.FindGameObjectsWithTag("BallBleue");
        GameObject[] ballsRouge = GameObject.FindGameObjectsWithTag("BallRouge");
        GameObject[] ballsBlack = GameObject.FindGameObjectsWithTag("Ball noire");
        foreach (GameObject ball in ballsBleue)
        {
            Destroy(ball);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        foreach (GameObject ball in ballsRouge)
        {
            Destroy(ball);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        foreach (GameObject ball in ballsBlack)
        {
            Destroy(ball);
            yield return new WaitForSecondsRealtime(0.2f);
        }

        if (!GameMaster.twoPlayers)
        {
            GameMaster.setHighScore(GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>());
        }

        if (canRevive && !GameMaster.twoPlayers)
        {
            GameMaster.panelEndGame.SetActive(true);

            while (!skip && countSkip < 5)
            {
                progressBar.GetComponent<RectTransform>().localScale = new Vector3((5f - countSkip) / 5f, 1, 1);
                countSkip += 0.2f;
                yield return new WaitForSecondsRealtime(0.2f);
            }
            if (revive)
            {
                while (!finishedWatching)
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                canRevive = false;
                isDead = false;
                GameMaster.panelEndGame.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Debug.Log("no revive");
                GameMaster.restart();
            }
        }
        else
        {
            Debug.Log("no revive two times");
            GameMaster.restart();
        }
    }

    public void startGame()
    {
        PlayerPrefs.SetInt("nbGame", PlayerPrefs.GetInt("nbGame") + 1);
        Advertisement.Banner.Hide();
        if (!GameMaster.twoPlayers)
        {
            timerPanel.SetActive(true);
        }
        else
        {
            scorePanel.SetActive(true);
            updateScore();
        }
        spawner.SetActive(true);
        UIMenu.SetActive(false);
    }

    public static void restart()
    {
        if (!GameMaster.twoPlayers)
        {
            string lastTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>().text.Split(':')[1].Trim();
            string timesString = PlayerPrefs.GetString("times");
            string[] times = timesString.Split(',');
            if (times.Length == 1 && times[0]=="")
            {
                timesString += lastTime;
            }
            else if(times.Length < 10)
            {
                timesString += ","+ lastTime;
            }
            else
            {
                for(int i = 1; i < times.Length; i++)
                {
                    times[i - 1] = times[i];
                }
                times[times.Length - 1] = lastTime;
                timesString = "";
                float moy = 0;
                for(int i = 0; i < times.Length; i++)
                {
                    moy += int.Parse(times[i]);
                    if(i == 0)
                    {
                        timesString = times[i];
                    }
                    else
                    {
                        timesString += "," + times[i];
                    }
                }
                moy /= times.Length;
                PlayerPrefs.SetFloat("moyenne", moy);
            }
            PlayerPrefs.SetString("times", timesString);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static int getHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static void setHighScore(Text timer)
    {
        string timerText = timer.text.Split(':')[1];
        int time = int.Parse(timerText);
        if(time > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", time);
        }
    }

    public static void updateScore()
    {
        Text scoreBlue = GameObject.FindGameObjectWithTag("ScoreBlue").GetComponent<Text>();
        Text scoreRed = GameObject.FindGameObjectWithTag("ScoreRed").GetComponent<Text>();
        scoreBlue.text = GameMaster.lifeBlue.ToString() + " ♥ ";
        scoreRed.text = "♥ " + GameMaster.lifeRed;
    }

    public void skipVideo()
    {
        skip = true;
    }

}
