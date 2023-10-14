using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SuperMarioManager : Singleton<SuperMarioManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gamePause;
    public UnityEvent gameResume;
    public UnityEvent levelRestart;
    public UnityEvent gameRestart;
    public UnityEvent marioDeath;
    public UnityEvent gameOver;
    public UnityEvent scoreChange;
    public UnityEvent livesChange;

    private GameObject marioBody;
    public static Vector3 marioPosition;

    public AudioMixer audioMixer;
    private AudioMixerSnapshot audioMixerDefaultSnapshot;
    private float specialEventsPitch = 0.95f;

    public GameConstants gameConstants;
    public IntVariable lives;
    public IntVariable score;

    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        gameStart.Invoke();
        Time.timeScale = 1.0f;
        audioMixerDefaultSnapshot = audioMixer.FindSnapshot("Default");
        audioMixerDefaultSnapshot.TransitionTo(0.1f);

        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;

        marioBody = GameObject.Find("Mario");

        SetScore();
        SetLives();

        lives.SetValue(gameConstants.maxLives);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore();
    }

    public void GamePause()
    {
        gamePause.Invoke();
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        gameResume.Invoke();
        Time.timeScale = 1f;
    }

    public void LevelRestart()
    {

    }

    public void GameRestart()
    {
        SceneManager.LoadSceneAsync("World 1-1", LoadSceneMode.Single);

        score.SetValue(0);
        lives.SetValue(gameConstants.maxLives);

        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        ResetAudioMixerSpecialEventsPitch();
    }

    public void MarioDeath()
    {
        marioDeath.Invoke();

        lives.ApplyChange(-1);
        SetLives();

        if (lives.Value < 1)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public void IncreaseScore(int increment)
    {
        // score += increment;
        // 
        // IncreaseAudioMixerSpecialEventsPitch();

        score.ApplyChange(1);
        SetScore();
    }

    public void IncreaseAudioMixerSpecialEventsPitch()
    {
        specialEventsPitch += 0.025f;
        audioMixer.SetFloat("SpecialEventsPitch", specialEventsPitch);
    }

    public void SmallMarioPowerUp()
    {
        marioBody.GetComponent<PlayerMovement>().SmallMarioPowerUp();
    }


    public void ResetAudioMixerSpecialEventsPitch()
    {
        specialEventsPitch = 0.975f;
        audioMixer.SetFloat("SpecialEventsPitch", 1f);
    }

    public void SetScore()
    {
        scoreChange.Invoke();
    }

    public void SetLives()
    {
        livesChange.Invoke();
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0.0f;
        gameOver.Invoke();
        score.Value = 0;
    }
}