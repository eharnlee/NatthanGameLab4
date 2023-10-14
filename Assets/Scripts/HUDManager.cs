using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-645, 600, 0),
        new Vector3(0, 75, 0)
        };
    private Vector3[] highScoreTextPosition = {
        new Vector3(-645, 525, 0),
        new Vector3(0, 10, 0)
        };
    private Vector3[] pauseButtonPosition = {
        new Vector3(1000, 590, 0)
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(1120, 590, 0),
        new Vector3(0, -85, 0)
    };

    public IntVariable gameScore;
    public IntVariable lives;

    private GameObject gamePausedPanel;
    private GameObject gameOverPanel;
    private GameObject scoreText;
    private GameObject highScoreText;
    private GameObject livesText;
    private GameObject pauseButton;
    private GameObject restartButton;

    void Awake()
    {
        // subscribe to events
        SuperMarioManager.instance.gameStart.AddListener(GameStart);
        SuperMarioManager.instance.gamePause.AddListener(GamePause);
        SuperMarioManager.instance.gameResume.AddListener(GameResume);
        SuperMarioManager.instance.gameRestart.AddListener(GameStart);
        SuperMarioManager.instance.gameOver.AddListener(GameOver);
        SuperMarioManager.instance.livesChange.AddListener(SetLives);
        SuperMarioManager.instance.scoreChange.AddListener(SetScore);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString("D6");
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString("D6");
    }

    public void SetLives()
    {
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives: " + lives.Value.ToString();
    }

    public void GameStart()
    {
        gamePausedPanel = this.transform.Find("GamePausedPanel").gameObject;
        gameOverPanel = this.transform.Find("GameOverPanel").gameObject;

        scoreText = this.transform.Find("ScoreText").gameObject;
        highScoreText = this.transform.Find("HighScoreText").gameObject;
        livesText = this.transform.Find("LivesText").gameObject;

        pauseButton = this.transform.Find("PauseButton").gameObject;
        restartButton = this.transform.Find("RestartButton").gameObject;

        // hide gameover panel
        gamePausedPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        scoreText.transform.localPosition = scoreTextPosition[0];
        scoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        highScoreText.transform.localPosition = highScoreTextPosition[0];
        highScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        livesText.SetActive(true);

        pauseButton.SetActive(true);
        pauseButton.transform.localPosition = pauseButtonPosition[0];
        restartButton.transform.localPosition = restartButtonPosition[0];

        SetScore();
        SetLives();
    }

    public void GamePause()
    {
        gamePausedPanel.SetActive(true);
        pauseButton.SetActive(false);

    }

    public void GameResume()
    {
        gamePausedPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        scoreText.transform.localPosition = scoreTextPosition[1];
        scoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        highScoreText.transform.localPosition = highScoreTextPosition[1];
        highScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        livesText.SetActive(false);

        pauseButton.SetActive(false);
        restartButton.transform.localPosition = restartButtonPosition[1];

        // set highscore
        // highscoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString("D6");
        // // show
        // highscoreText.SetActive(true);
    }
}