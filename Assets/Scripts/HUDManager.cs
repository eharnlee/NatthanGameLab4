using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-937, 604, 0),
        new Vector3(0, -100, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(1114, 592, 0),
        new Vector3(0, -200, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;

    public GameObject gameOverPanel;

    void Awake()
    {
        // subscribe to events
        SuperMarioManager.instance.gameStart.AddListener(GameStart);
        SuperMarioManager.instance.gameOver.AddListener(GameOver);
        SuperMarioManager.instance.gameRestart.AddListener(GameStart);
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

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
    }
}