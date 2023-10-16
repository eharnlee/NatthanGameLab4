using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuHUDManager : MonoBehaviour
{
    private GameObject highScoreText;
    public IntVariable score;

    void Awake()
    {
        SuperMarioManager.instance.loadScene.AddListener(LoadScene);
        SuperMarioManager.instance.scoreChange.AddListener(SetScore);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNewGameButton()
    {
        SuperMarioManager.instance.StartNewGame();
    }
    public void LoadScene()
    {
        highScoreText = this.transform.Find("HighScoreText").gameObject;
        SetScore();
    }

    public void SetScore()
    {
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + score.previousHighestValue.ToString("D6");
    }

    public void ResetHighScore()
    {
        SuperMarioManager.instance.ResetHighScore();
    }
}
