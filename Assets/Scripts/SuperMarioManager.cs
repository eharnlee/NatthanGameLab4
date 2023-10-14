using System.Collections;
using System.Collections.Generic;
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
    public UnityEvent gameRestart;
    public UnityEvent gameOver;
    public UnityEvent<int> scoreChange;

    public static Vector3 marioPosition;
    private GameObject marioBody;
    private int score = 0;
    public AudioMixer audioMixer;
    private AudioMixerSnapshot audioMixerDefaultSnapshot;
    private float specialEventsPitch = 0.95f;

    public IntVariable gameScore;

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

        SetScore(score);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(score);
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
    public void GameRestart()
    {
        gameScore.Value = 0;

        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        ResetAudioMixerSpecialEventsPitch();
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

        gameScore.ApplyChange(1);
        SetScore(gameScore.Value);
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

    public void SetScore(int score)
    {
        // scoreChange.Invoke(score);
        scoreChange.Invoke(gameScore.Value);
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0.0f;
        gameOver.Invoke();
        gameScore.Value = 0;
    }
}